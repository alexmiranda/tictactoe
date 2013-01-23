using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Grid
{
    [ContractClassFor(typeof(IGrid<>))]
    internal abstract class GridContracts<T> : IGrid<T> where T : IPosition
    {
        public IEnumerable<IRow> CompletedRows()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IRow>>() != null);
            return default(IEnumerable<IRow>);
        }

        bool IGrid<T>.IsBlank(T position)
        {
            Contract.Requires<ArgumentNullException>(position != null, "position");
            return default(bool);
        }

        bool IGrid<T>.IsFilled(T position)
        {
            Contract.Requires<ArgumentNullException>(position != null, "position");
            return default(bool);
        }

        Mark IGrid<T>.this[T position]
        {
            get { return default(Mark); }
            set { Contract.Requires<ArgumentNullException>(position != null, "value"); }
        }
        
        public abstract void Reset();
    }
}
