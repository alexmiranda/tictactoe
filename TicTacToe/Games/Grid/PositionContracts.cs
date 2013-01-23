using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Grid
{
    [ContractClassFor(typeof(IPosition))]
    internal abstract class PositionContracts : IPosition
    {
        int IPosition.Row
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }

        int IPosition.Col
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }
    }
}
