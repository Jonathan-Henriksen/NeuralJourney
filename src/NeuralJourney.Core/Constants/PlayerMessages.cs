﻿namespace NeuralJourney.Core.Constants
{
    public static class PlayerMessages
    {
        public const string SomethingWentWrong = "Something went wrong. Please try again";

        public const string UnexpectedError = "An unexpected error occured";

        public static class Command
        {
            public const string NoMatch = "Could not match the input to any command. Please try rephrasing it";

            public const string CompletionTextFailure = "The server could not connect to the OpenAI API. Please try agian";
        }

        public static class Message
        {
            public const string TooLarge = "The length of the provided message({MessageSize} characters) exceeds the allowed length of {MessageSizeLimit}. Please shorten your message and try again";

            public const string TooSmall = "The provided message is too short. Please provide more details and try again";

            public const string InvalidFormat = "The format of your message is not valid. Please check and resend";
        }

        public static class WelcomeFlow
        {
            public const string EnterName = "Please enter your name";

            public const string WelcomeMessage = "Welcome to Neural Journey! Please enter your name";

            public const string WelcomeNameMessage = "Hi {0}! Welcome to Neural Journey. Let's get started";

            public const string NameAlreadyTaken = "Sorry that name is already taken. Please choose another";

            public const string InvalidNameFormat = "Invalid name. Use only letters, numbers, underscores, and hyphens. Length should be between 3 to 16 characters";
        }

        public static class Map
        {
            public const string LimitReached = "You have reached the limit in this direction. Please explore a different path";
        }

        public static class Network
        {
            public const string ConnectionLost = "Connection lost. Please check your network and try reconnecting";

            public const string ConnectionError = "A network error occurred. Please try again later";

            public const string RetryLimitReached = "The program reached the try limit of {RetryLimit} while processing {ProcessName}}";
        }
    }
}