﻿using NeuralJourney.Library.Models.World;
using NeuralJourney.Logic.Dispatchers.Interfaces;
using NeuralJourney.Logic.Engines.Interfaces;
using NeuralJourney.Logic.Handlers.Interfaces;
using NeuralJourney.Logic.Services.Interfaces;

namespace NeuralJourney.Logic.Engines
{
    public class GameEngine : IGameEngine
    {
        private readonly IClockService _clock;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IInputHandler _inputHandler;
        private readonly IConnectionHandler _connectionHandler;

        private readonly List<Player> _players = new();

        public GameEngine(IClockService clockService, ICommandDispatcher commandDispatcher, IInputHandler inputHandler, IConnectionHandler connectionHandler)
        {
            _clock = clockService;
            _commandDispatcher = commandDispatcher;
            _connectionHandler = connectionHandler;
            _inputHandler = inputHandler;

            _connectionHandler.OnPlayerConnected += AddPlayer;

            _inputHandler.OnPlayerInputReceived += _commandDispatcher.DispatchPlayerCommandAsync;
            _inputHandler.OnAdminInputReceived += _commandDispatcher.DispatchAdminCommandAsync;
        }

        public async Task Run()
        {
            _clock.Start();

            var connectionHandlerTask = _connectionHandler.HandleAsync();
            var inputHandlerTask = _inputHandler.HandleAdminInputAsync();

            await Task.WhenAll(connectionHandlerTask, inputHandlerTask);
        }

        public void Stop()
        {
            _clock.Reset();

            _connectionHandler.Stop();

            foreach (var player in _players)
            {
                player.GetStream()?.Close();
                _players.Remove(player);
            }
        }

        private void AddPlayer(Player player)
        {
            _players.Add(player);

            _ = _inputHandler.HandlePlayerInputAsync(player);
        }
    }
}