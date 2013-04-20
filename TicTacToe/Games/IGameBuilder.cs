using TicTacToe.Games.Rules;

namespace TicTacToe.Games
{
    public interface IGameBuilder
    {
        IGameBuilder With(IRuleAssistant assistant);
        IGame StartNew(string kind);
    }
}
