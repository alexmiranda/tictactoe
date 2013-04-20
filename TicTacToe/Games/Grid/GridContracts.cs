using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Grid
{
    [ContractClassFor(typeof(IGrid))]
    internal abstract class GridContracts : IGrid
    {
        public Mark Get(IPosition position)
        {
            Contract.Requires<ArgumentNullException>(position != null, "position");
            return default(Mark);
        }

        IGrid IGrid.Fill(IPosition position, Mark mark)
        {
            Contract.Requires<ArgumentNullException>(position != null, "position");
            Contract.Requires<ArgumentException>(mark != Mark.Blank, "mark");
            return default(IGrid);
        }

        bool IGrid.IsBlank(IPosition position)
        {
            Contract.Requires<ArgumentNullException>(position != null, "position");
            return default(bool);
        }

        bool IGrid.IsFilled(IPosition position)
        {
            Contract.Requires<ArgumentNullException>(position != null, "position");
            return default(bool);
        }

        IEnumerable<IRow> IGrid.CompletedRows()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IRow>>() != null);
            return default(IEnumerable<IRow>);
        }
    }
}
