using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Grid
{
    [ContractClass(typeof(GridContracts))]
    public interface IGrid
    {
        Mark Get(IPosition position);
        IGrid Fill(IPosition position, Mark mark);
        bool IsBlank(IPosition position);
        bool IsFilled(IPosition position);
        IEnumerable<IRow> CompletedRows();
    }
}