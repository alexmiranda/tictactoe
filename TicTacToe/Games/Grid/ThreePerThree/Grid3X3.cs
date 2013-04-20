using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Games.Grid.ThreePerThree
{
    internal class Grid3X3 : IGrid
    {
        private readonly Mark[,] _grid = new[,] { { Mark.Blank, Mark.Blank, Mark.Blank }, 
                                                  { Mark.Blank, Mark.Blank, Mark.Blank }, 
                                                  { Mark.Blank, Mark.Blank, Mark.Blank } };

        public Grid3X3() { }

        private Grid3X3(Mark[,] grid) : this()
        {
            _grid = grid;
        }

        public virtual Mark Get(IPosition position)
        {
            return _grid[position.Row, position.Col];
        }

        public virtual IGrid Fill(IPosition position, Mark mark)
        {
            var grid = new Mark[3, 3];
            Array.Copy(_grid, grid, _grid.Length);
            grid[position.Row, position.Col] = mark;
            return new Grid3X3(grid);
        }

        public virtual bool IsBlank(IPosition position)
        {
            return Get(position) == Mark.Blank;
        }

        public virtual bool IsFilled(IPosition position)
        {
            return !IsBlank(position);
        }

        public IEnumerable<IRow> CompletedRows()
        {
            return
                from r in Rows.GetAll<Rows3X3>()
                from p in r.Positions
                group p by r into g
                where g.All(x => Get(x) == Mark.Cross)
                   || g.All(x => Get(x) == Mark.Nought)
                select g.Key;
        } 
    }
}