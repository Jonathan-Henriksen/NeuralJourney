﻿using System.Text.RegularExpressions;

namespace NeuralJourney.Library.Exceptions
{
    [Serializable]
    public abstract class GameException : Exception
    {
        public readonly string? UserFriendlyMessage;
        public readonly string? MessageTemplate;

        protected GameException(string userFriendlyMessage, string messageTemplate, params object?[] formatArgs)
            : base(string.Format(TransformTemplate(messageTemplate), formatArgs))
        {
            UserFriendlyMessage = userFriendlyMessage;
            MessageTemplate = messageTemplate;
        }

        public GameException() { }
        public GameException(string message) : base(message) { }
        public GameException(string message, Exception inner) : base(message, inner) { }
        protected GameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string TransformTemplate(string serilogTemplate)
        {
            var regex = new Regex(@"{(\w+)}");
            int index = 0;
            return regex.Replace(serilogTemplate, m => $"{{{index++}}}");
        }
    }
}