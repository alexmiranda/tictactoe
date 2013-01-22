namespace TicTacToe.Games.Selectors
{
    public abstract class PlayerSelector : IPlayerSelector
    {
        private static readonly PlayerSelector DefaultPlayerSelector = new TraditionalPlayerSelector();

        public static PlayerSelector Null { get { return DefaultPlayerSelector; } }

        private Mark _mark = Mark.Blank;

        public Mark Next()
        {
            if (_mark == Mark.Blank)
                return _mark = GetFirstMark();
            return _mark.Switch();
        }

        protected abstract Mark GetFirstMark();
    }
}
