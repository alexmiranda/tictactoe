namespace TicTacToe.Games.Rules
{
    public interface IRuleAssistant
    {
        void SetGame(IGame game);
        void AcceptMove(Move move);
    }
}
