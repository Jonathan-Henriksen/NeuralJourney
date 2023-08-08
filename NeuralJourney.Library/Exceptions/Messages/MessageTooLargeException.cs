﻿using NeuralJourney.Library.Constants;

namespace NeuralJourney.Library.Exceptions.Messages
{
    [Serializable]
    public class MessageTooLargeException : GameException
    {
        public readonly string MessageText;

        public readonly int CharacterLimit;

        public MessageTooLargeException(string messageText, int characterLimit) :
            base(PlayerMessageTemplates.Message.TooLarge, ErrorMessageTemplates.Message.TooLarge, messageText.Length, characterLimit)
        {
            MessageText = messageText;
            CharacterLimit = characterLimit;
        }
    }
}