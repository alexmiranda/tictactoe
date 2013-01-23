using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Games.Grid.ThreePerThree
{
    internal class Grid3X3 : IGrid<Positions3X3>
    {
        private readonly Mark[,] _grid = new[,] { { Mark.Blank, Mark.Blank, Mark.Blank }, 
                                                  { Mark.Blank, Mark.Blank, Mark.Blank }, 
                                                  { Mark.Blank, Mark.Blank, Mark.Blank } };
        
        public virtual IEnumerable<IRow> CompletedRows()
        {
            return
                from r in Rows.GetAll<Rows3X3>()
                from p in r.Positions
                group p by r into g
                where g.All(x => this[x as Positions3X3] == Mark.Cross)
                   || g.All(x => this[x as Positions3X3] == Mark.Nought)
                select g.Key;
        }

        public virtual bool IsBlank(Positions3X3 position)
        {
            return _grid[position.Row, position.Col] == Mark.Blank;
        }

        public virtual bool IsFilled(Positions3X3 position)
        {
            return !IsBlank(position);
        }

        public virtual Mark this[Positions3X3 position]
        {
            get { return _grid[position.Row, position.Col]; }
            set { _grid[position.Row, position.Col] = value; }
        }

        public virtual void Reset()
        {
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    _grid[i, j] = Mark.Blank;
        }
    }
}