﻿using NeuralJourney.Core.Exceptions;
using NeuralJourney.Core.Extensions;
using NeuralJourney.Core.Interfaces.Commands;
using NeuralJourney.Core.Interfaces.Services;
using NeuralJourney.Core.Models.Commands;
using Serilog;

namespace NeuralJourney.Core.Commands.Players
{
    public class PlayerCommandStrategy : ICommandStrategy
    {
        private readonly ICommandMiddleware[] _middlewareProcessors;
        private readonly IMessageService _messageService;
        private readonly ILogger _logger;

        public PlayerCommandStrategy(IEnumerable<ICommandMiddleware> commandMiddleware, IMessageService messageService, ILogger logger)
        {
            _middlewareProcessors = commandMiddleware.ToArray();

            _messageService = messageService;

            _logger = logger.ForContext<PlayerCommandStrategy>();
        }

        public async Task ExecuteAsync(CommandContext context, CancellationToken cancellationToken = default)
        {
            var errorMessage = string.Empty;

            try
            {
                if (context.Player is null)
                    throw new CommandExecutionException("Player was null");

                var index = -1;

                async Task Next()
                {
                    if (++index < _middlewareProcessors.Length)
                        await _middlewareProcessors[index].InvokeAsync(context, Next, cancellationToken);
                }

                await Next();
            }
            catch (CommandMappingException ex)
            {
                _logger.Error(ex, "Failed to create command");
                errorMessage = ex.PlayerMessage;
            }
            catch (CommandExecutionException ex)
            {
                _logger.Error(ex, "Error while executing command");
                errorMessage = ex.PlayerMessage;
            }
            catch (OperationCanceledException)
            {
                return; // Do nothing on intended cancellation. Player Handler closes connections gracefully
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error while processing the command");
                errorMessage = "An unexpected error occured. Please try again";
            }
            finally
            {
                if (!string.IsNullOrEmpty(errorMessage) && context.Player is not null)
                    await _messageService.SendMessageAsync(context.Player.GetClient(), errorMessage, cancellationToken);
            }
        }
    }
}