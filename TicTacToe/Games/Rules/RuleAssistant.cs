namespace TicTacToe.Games.Rules
{
    public abstract class RuleAssistant : IRuleAssistant
    {
        internal static readonly IRuleAssistant Null = new SlackAssistant();

        protected internal IGame Game;

        public virtual void SetGame(IGame game)
        {
            Game = game;
        }

        public abstract void AcceptMove(Move move);
    }
}
