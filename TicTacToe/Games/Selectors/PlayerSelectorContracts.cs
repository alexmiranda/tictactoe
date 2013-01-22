using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Selectors
{
    [ContractClassFor(typeof(IPlayerSelector))]
    internal abstract class PlayerSelectorContracts : IPlayerSelector
    {
        public Mark Next()
        {
            Contract.Ensures(Contract.Result<Mark>() != Mark.Blank, "The next mark cannot be blank");
            return default(Mark);
        }
    }
}
