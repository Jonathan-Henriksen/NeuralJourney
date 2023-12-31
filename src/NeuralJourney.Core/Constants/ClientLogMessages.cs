﻿namespace NeuralJourney.Core.Constants
{
    public static class ClientLogMessages
    {
        public static class Debug
        {

        }

        public static class Info
        {
            public const string ConnectionClosed = "Server closed the connection";

            public const string ConnectionEstablished = "Connection established with the server";

            public const string ConnectionInitialize = "Connecting to the server...";

            public const string StartingGame = "Started the game";

            public const string StoppingGame = "Stopped the game";
        }

        public static class Error
        {
            public const string ConnectionFailed = "Failed to connect to the server";

            public const string ConnectionFailedTimeout = "Connection to the server timed out";

            public const string ConnectionFailedUnexpectedly = "Unexpected error while connecting to the server";
        }
    }

}