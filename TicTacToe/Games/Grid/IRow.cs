using System.Collections.Generic;

namespace TicTacToe.Games.Grid
{
    public interface IRow
    {
        IList<IPosition> Positions { get; }
        bool Includes(IPosition position);
    }
}
