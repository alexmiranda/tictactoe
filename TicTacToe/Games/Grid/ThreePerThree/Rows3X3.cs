using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TicTacToe.Games.Grid.ThreePerThree
{
    internal sealed class Rows3X3 : IRow
    {
        // Horizontal Rows
        internal static readonly Rows3X3 TopHorizontal = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.TopLeftCorner, Positions3X3.TopEdge, Positions3X3.TopRightCorner }
        };
        internal static readonly Rows3X3 CenterHorizontal = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.LeftEdge, Positions3X3.Center, Positions3X3.RightEdge }
        };
        internal static readonly Rows3X3 BottomHorizontal = new Rows3X3() { 
            Positions = new List<IPosition>() { Positions3X3.BottomLeftCorner, Positions3X3.BottomEdge, Positions3X3.BottomRightCorner } 
        };

        //// Vertical Rows
        internal static readonly Rows3X3 LeftVertical = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.TopLeftCorner, Positions3X3.RightEdge, Positions3X3.BottomLeftCorner }
        };
        internal static readonly Rows3X3 CenterVertical = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.TopEdge, Positions3X3.Center, Positions3X3.BottomEdge }
        };
        internal static readonly Rows3X3 RightVertical = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.TopRightCorner, Positions3X3.RightEdge, Positions3X3.BottomRightCorner }
        };

        //// Diagonal Rows
        internal static readonly Rows3X3 MainDiagonal = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.TopLeftCorner, Positions3X3.Center, Positions3X3.BottomRightCorner }
        };
        internal static readonly Rows3X3 CounterDiagonal = new Rows3X3()
        {
            Positions = new List<IPosition>() { Positions3X3.BottomLeftCorner, Positions3X3.Center, Positions3X3.TopRightCorner }
        };

        private Rows3X3()
        {
        }

        public IList<IPosition> Positions { get; private set; }

        public bool Includes(IPosition position)
        {
            return Positions.Contains(position);
        }
    }

    internal static class Rows
    {
        internal static IEnumerable<T> GetAll<T>() where T : IRow
        {
            return typeof (T).GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.IsAssembly && x.IsInitOnly)
                .Select(x => (T) x.GetValue(new object()));
        } 

        internal static IEnumerable<T> OfPosition<T>(IPosition position) where T : IRow
        {
            return GetAll<T>().Where(x => x.Includes(position));
        } 
    }
}
