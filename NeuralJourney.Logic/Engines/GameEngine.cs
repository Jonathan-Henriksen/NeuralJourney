﻿using NeuralJourney.Library.Models.World;
using NeuralJourney.Logic.Engines.Interfaces;
using NeuralJourney.Logic.Handlers.Interfaces;
using NeuralJourney.Logic.Services.Interfaces;

namespace NeuralJourney.Logic.Engines
{
    public class GameEngine : IGameEngine
    {
        private readonly IClockService _clock;
        private readonly IInputHandler _inputHandler;
        private readonly IConnectionHandler _connectionHandler;

        private readonly List<Player> _players = new();

        public GameEngine(IClockService clockService, IInputHandler inputHandler, IConnectionHandler playerConnectionHandler)
        {
            _clock = clockService;
            _inputHandler = inputHandler;
            _connectionHandler = playerConnectionHandler;

            _connectionHandler.Connected += AddPlayer;
        }

        public async Task Run()
        {
            var connectionHandlerTask = _connectionHandler.HandleAsync();
            var inputHandlerTask = _inputHandler.HandleAdminInputAsync();

            _clock.Start();

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