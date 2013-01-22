using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Grid
{
    [ContractClass(typeof(GridContracts<>))]
    internal interface IGrid<T> where T : IPosition
    {
        IEnumerable<IRow> CompletedRows();
        bool IsBlank(T position);
        bool IsFilled(T position);
        Mark this[T position] { get; set; }
        void Reset();
    }
}