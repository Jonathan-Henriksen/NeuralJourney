﻿using NeuralJourney.Core.Enums.Commands;
using NeuralJourney.Core.Exceptions;
using NeuralJourney.Core.Interfaces.Commands;
using NeuralJourney.Core.Models.LogProperties;
using System.Text.RegularExpressions;

namespace NeuralJourney.Core.Commands.Players.Middleware
{
    public partial class CompletionTextParser : ICommandMiddleware
    {
        public async Task InvokeAsync(CommandContext context, Func<Task> next, CancellationToken cancellationToken = default)
        {
            var completionText = context.CompletionText ?? throw new CommandMappingException("Completion text was blank");

            var match = CompletionTextMatcher().Match(completionText);

            if (!match.Success)
                throw new CommandMappingException("Completion text did not match the expected format");

            var commandIdentifierText = match.Groups["commandIdentifier"].Value;
            if (!Enum.TryParse(commandIdentifierText, true, out CommandIdentifierEnum commandIdentifier))
                throw new CommandMappingException("Failed to parse commandIdentifier to enum");

            context.CommandKey.Identifier = commandIdentifier;

            context.Params = match.Groups["params"].Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            context.ExecutionMessage = match.Groups["successMessage"].Value;

            if (string.IsNullOrEmpty(context.ExecutionMessage))
                throw new CommandMappingException("Execution message was blank");

            await next();
        }

        [GeneratedRegex(@"^(?<commandIdentifier>\w+)(\((?<params>[^\)]*)\))?(\|(?<successMessage>.+?))?$")]
        private static partial Regex CompletionTextMatcher();
    }
}