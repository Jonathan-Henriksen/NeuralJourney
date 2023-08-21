﻿using NeuralJourney.Core.Enums.Commands;
using NeuralJourney.Core.Exceptions;
using NeuralJourney.Core.Interfaces.Commands;
using NeuralJourney.Core.Models.Commands;
using NeuralJourney.Core.Models.LogProperties;
using NeuralJourney.Core.Models.Options;
using System.Text;

namespace NeuralJourney.Core.Commands.Players.Commands
{
    [Command(CommandTypeEnum.Player, CommandIdentifierEnum.CheckMap)]
    public class CheckMapCommand : ICommand
    {
        private readonly CommandContext _context;

        private readonly int _worldWidth;
        private readonly int _worldHeight;

        private const char _wallChar = '#';
        private const char _floorChar = '-';
        private const char _playerChar = 'p';

        public CheckMapCommand(CommandContext context, GameOptions gameOptions)
        {
            _context = context;

            _worldWidth = gameOptions.WorldWidth;
            _worldHeight = gameOptions.WorldHeight;
        }

        public Task<CommandResult> ExecuteAsync()
        {
            return Task.Run(() =>
            {
                if (_context.Player is null)
                    throw new CommandExecutionException("The player was null", "Something went wrong. Please try again");

                var mapBuilder = new StringBuilder();
                mapBuilder.AppendLine(new string(_wallChar, _worldWidth + 2));
                for (var y = 0; y < _worldHeight; y++)
                {
                    mapBuilder.Append(_wallChar);
                    for (var x = 0; x < _worldWidth; x++)
                    {
                        if (x == _context.Player.Location.X && y == _context.Player.Location.Y)
                            mapBuilder.Append(_playerChar);
                        else
                            mapBuilder.Append(_floorChar);
                    }
                    mapBuilder.AppendLine($"{_wallChar}");
                }
                mapBuilder.Append(new string(_wallChar, _worldWidth + 2));
                return new CommandResult(mapBuilder.ToString());
            });
        }

    }
}