using TicTacToe.Games;
using TicTacToe.Games.Rules;
using TicTacToe.Player;

namespace TicTacToe.Games
{
    public interface IGameBuilder
    {
        IPlayerBuilder Playing(string name);
        IGameBuilder With(IPlayer player);
        IGameBuilder Against(IPlayer player);
        IGameBuilder WaitForOpponentToJoin();
        IGameBuilder AssistedBy(IRuleAssistant assistant);
        IGame Build();
    }

    public interface IPlayerBuilder
    {
        IPlayerContinue IdentifiedBy(Mark mark);
    }

    public interface IPlayerContinue
    {
        IGameBuilder And { get; }
    }

    public interface IOpponentBuilder
    {
        IGameBuilder AgainstTheComputer();
        IGameBuilder WaitForTheOpponentToJoin();
        IPlayerBuilder Against();
    }
}
