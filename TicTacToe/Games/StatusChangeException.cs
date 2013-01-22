using System;

namespace TicTacToe.Games
{
    public class StatusChangeException : InvalidOperationException
    {
        public StatusChangeException(string message)
            : base(message)
        {
        }
    }
}
