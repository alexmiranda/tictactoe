using System;

namespace TicTacToe.Games
{
    [Serializable]
    public class BadMoveException : InvalidOperationException
    {
        public BadMoveException() { }

        public BadMoveException(string message)
            : base(message)
        {
        }
    }

    public class GameSequenceException : BadMoveException
    {
        public GameSequenceException(string message)
            : base(message)
        {    
        }

        public GameSequenceException()
            : base("Move was not accepted because the opponent must play first")
        {
        }
    }

    public class GameNotStartedException : GameSequenceException
    {
        public GameNotStartedException()
            : base("The game has not been started yet")
        {
        }
    }

    public class FilledPositionException : BadMoveException
    {
        public FilledPositionException()
            : base("The position is already filled")
        {

        }
    }
}
