namespace TicTacToe.Games
{
    public class GameStatus
    {
        internal static readonly GameStatus New = new GameStatus();
        private static readonly GameStatus Started = new GameStatus(true, false);
        private static readonly GameStatus Over = new GameStatus(false, true);

        private readonly bool _started;
        private readonly bool _over;

        private GameStatus(bool started = false, bool over = false)
        {
            _started = started;
            _over = over;
        }

        public bool IsStarted()
        {
            return _started;
        }

        public bool IsOver()
        {
            return _over;
        }

        public GameStatus ToStarted()
        {
            if (this == New)
                return Started;
            throw new StatusChangeException(string.Format("Cannot change from {0} to Started", this));
        }

        public GameStatus ToOver()
        {
            if (this == Started)
                return Over;
            throw new StatusChangeException(string.Format("Cannot change from {0} to Over", this));
        }

        public override string ToString()
        {
            if (this == New)
                return "New";
            if (this == Started)
                return "Started";
            if (this == Over)
                return "Over";

            return base.ToString();
        }
    }
}
