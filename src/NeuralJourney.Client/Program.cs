﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NeuralJourney.Core.Interfaces.Engines;
using NeuralJourney.Core.Interfaces.Handlers;
using NeuralJourney.Core.Interfaces.Services;
using NeuralJourney.Core.Models.Options;
using NeuralJourney.Infrastructure.Engines;
using NeuralJourney.Infrastructure.Handlers;
using NeuralJourney.Infrastructure.Services;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using System.Net.Sockets;

using var host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
using var cts = new CancellationTokenSource();

var services = scope.ServiceProvider;

var logger = services.GetRequiredService<ILogger>();

try
{
    var engine = services.GetRequiredService<IEngine>();
    Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e) // Gracefully shuut down on request by user
    {
        logger.Debug("Client initializd shutdown");

        e.Cancel = true;

        engine.StopAsync();
        cts.Cancel();
    };

    await engine.Run(cts.Token);
}
catch (OperationCanceledException)
{
    // Handling and logging is handled in the engine on intended cancellation
}
catch (Exception ex)
{
    cts.Cancel();
    logger.Fatal(ex, "Client engine crashed unexpectedly"); // Unexpected error that crashed the engine
}
finally
{
    cts.Dispose();

    logger.Debug("Shutting down client engine");
    await Task.Delay(2000); // Let the message hang for 2 seconds before closing the window
}

// Configure Application
static IHostBuilder CreateHostBuilder(string[] strings)
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.json", false);
        })
        .UseSerilog((hostingContext, loggerConfiguration) =>
        {
            var options = hostingContext.Configuration.GetRequiredSection("Client:Serilog").Get<SerilogOptions>()
                ?? throw new InvalidOperationException("Serilog configuration is missing");

            if (string.IsNullOrEmpty(options.SeqUrl))
                throw new InvalidOperationException("Serilog configuration is missing Seq URL");

            var logLevel = Enum.TryParse(options.LogLevel, out LogEventLevel level) ? level : LogEventLevel.Information;

            loggerConfiguration
                .MinimumLevel.Is(logLevel)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers().WithIgnoreStackTraceAndTargetSiteExceptionFilter())
                .Enrich.WithProperty("Application", "Client")
                .WriteTo.Seq(serverUrl: options.SeqUrl, restrictedToMinimumLevel: logLevel
            );
        })
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IEngine, ClientEngine>();

            services.AddTransient<IMessageService, MessageService>();

            services.AddTransient<IInputHandler<TextReader>, ConsoleInputHandler>();
            services.AddTransient<IInputHandler<TcpClient>, NetworkInputHandler>();

            services.AddOptions<ClientOptions>().BindConfiguration("Client:Network");
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ClientOptions>>().Value);
        });
}