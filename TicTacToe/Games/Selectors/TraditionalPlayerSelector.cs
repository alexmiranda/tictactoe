namespace TicTacToe.Games.Selectors
{
    public class TraditionalPlayerSelector : PlayerSelector
    {
        internal TraditionalPlayerSelector()
        {
        }

        protected override Mark GetFirstMark()
        {
            return Mark.Cross;
        }
    }
}
