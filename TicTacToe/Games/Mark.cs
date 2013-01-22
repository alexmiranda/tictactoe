using System;
using TicTacToe.Games.Grid;

namespace TicTacToe.Games
{
    public enum Mark : short
    {
        Blank = 0,
        Cross = 1,
        Nought = 2
    }

    internal static class Marks
    {
        internal static void On<T>(this Mark mark, IGrid<T> grid, T position) where T : IPosition
        {
            if (grid != null && position != null) grid[position] = mark;
        }

        internal static Mark Switch(this Mark mark)
        {
            if (mark == Mark.Cross)
                return Mark.Nought;
            else if (mark == Mark.Nought)
                return Mark.Cross;
            throw new InvalidOperationException("Cannot switch from Blank");
        }
    }
}