using System.Collections.Generic;
using TicTacToe.Games;
using TicTacToe.Games.Grid;

namespace TicTacToe.UnitTests.Support.Fakes
{
    internal class FakeGrid : IGrid
    {
        private readonly Dictionary<IPosition, Mark> _state = new Dictionary<IPosition, Mark>();
        private IEnumerable<IRow> _completedRows = new List<IRow>(); 

        public virtual Mark Get(IPosition position)
        {
            return _state.ContainsKey(position) ? _state[position] : Mark.Blank;
        }

        public virtual IGrid Fill(IPosition position, Mark mark)
        {
            _state[position] = mark;
            return this;
        }

        public virtual bool IsBlank(IPosition position)
        {
            return Get(position) == Mark.Blank;
        }

        public virtual bool IsFilled(IPosition position)
        {
            return !IsBlank(position);
        }

        public void SetCompletedRows(IEnumerable<IRow> completedRows)
        {
            _completedRows = completedRows;
        }

        public IEnumerable<IRow> CompletedRows()
        {
            return _completedRows;
        }
    }
}
