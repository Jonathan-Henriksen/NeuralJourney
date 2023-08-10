﻿using NeuralJourney.Library.Constants;
using NeuralJourney.Logic.Handlers.Interfaces;
using NeuralJourney.Logic.Options;
using Serilog;
using System.Net;
using System.Net.Sockets;

namespace NeuralJourney.Logic.Handlers
{
    public class NetworkConnectionHandler : IConnectionHandler
    {
        private CancellationTokenSource _cts;
        private readonly TcpListener _tcpListener;

        private readonly ILogger _logger;

        public event Action<TcpClient>? OnConnected;

        public NetworkConnectionHandler(ServerOptions serverOptions, ILogger logger)
        {
            _cts = new CancellationTokenSource();
            _tcpListener = new TcpListener(IPAddress.Any, serverOptions.Port);
            _logger = logger;
        }

        public async Task HandleConnectionsAsync(CancellationToken cancellationToken = default)
        {
            _tcpListener.Start();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                _logger.Information(InfoMessageTemplates.ServerStarted);

                while (!_cts.Token.IsCancellationRequested)
                {
                    var client = await _tcpListener.AcceptTcpClientAsync(_cts.Token);

                    _logger.Information(InfoMessageTemplates.ClientConnected, client.Client.RemoteEndPoint);

                    if (client is null)
                        continue;

                    OnConnected?.Invoke(client);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            finally
            {
                _tcpListener.Stop();

                _logger.Information(InfoMessageTemplates.ServerStopped);
            }
        }

        public void Stop()
        {
            _tcpListener.Stop();
            _tcpListener.Server.Dispose();

            _cts.Cancel();
            _cts.Dispose();
        }
    }
}