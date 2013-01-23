using System;

namespace TicTacToe.Games.Selectors
{
    public class RandomPlayerSelector : PlayerSelector
    {
        protected override Mark GetFirstMark()
        {
            int next = new Random().Next(0, 1);
            if (next == 0) return Mark.Cross;
            if (next == 1) return Mark.Nought;
            throw new InvalidOperationException();
        }
    }
}
