using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Selectors
{
    [ContractClass(typeof(PlayerSelectorContracts))]
    public interface IPlayerSelector
    {
        Mark Next();
    }
}
