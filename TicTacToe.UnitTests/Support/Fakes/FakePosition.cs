using TicTacToe.Games.Grid;

namespace TicTacToe.UnitTests.Support.Fakes
{
    public class FakePosition : IPosition
    {
        public FakePosition(int row = 0, int col = 0)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; private set; }
        public int Col { get; private set; }
    }
}
