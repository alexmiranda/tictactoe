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
        internal static IGrid On<T>(this Mark mark, IGrid grid, T position) where T : IPosition
        {
            if (grid != null && position != null) 
                return grid.Fill(position, mark);
            return grid;
        }

        internal static Mark Switch(this Mark mark)
        {
            if (mark == Mark.Cross)
                return Mark.Nought;
            if (mark == Mark.Nought)
                return Mark.Cross;
            throw new InvalidOperationException("Cannot switch from Blank");
        }
    }
}