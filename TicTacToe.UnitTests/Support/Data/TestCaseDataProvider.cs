using NUnit.Framework;
using System.Collections.Generic;
using TicTacToe.Games;
using TicTacToe.Games.Grid.ThreePerThree;

namespace TicTacToe.UnitTests.Support.Data
{
    public static class TestCaseDataProvider
    {
        public static IEnumerable<TestCaseData> Marks
        {
            get
            {
                yield return new TestCaseData(Mark.Cross);
                yield return new TestCaseData(Mark.Nought);
            }
        } 

        public static IEnumerable<TestCaseData> Positions
        {
            get
            {
                yield return new TestCaseData(Positions3X3.TopLeftCorner);
                yield return new TestCaseData(Positions3X3.TopRightCorner);
                yield return new TestCaseData(Positions3X3.BottomLeftCorner);
                yield return new TestCaseData(Positions3X3.BottomRightCorner);
                yield return new TestCaseData(Positions3X3.Center);
                yield return new TestCaseData(Positions3X3.LeftEdge);
                yield return new TestCaseData(Positions3X3.TopEdge);
                yield return new TestCaseData(Positions3X3.RightEdge);
                yield return new TestCaseData(Positions3X3.BottomEdge);
            }
        } 

        public static IEnumerable<TestCaseData> Rows
        {
            get
            {
                yield return new TestCaseData(Rows3X3.TopHorizontal);
                yield return new TestCaseData(Rows3X3.CenterHorizontal);
                yield return new TestCaseData(Rows3X3.BottomHorizontal);
                yield return new TestCaseData(Rows3X3.LeftVertical);
                yield return new TestCaseData(Rows3X3.CenterVertical);
                yield return new TestCaseData(Rows3X3.RightVertical);
                yield return new TestCaseData(Rows3X3.MainDiagonal);
                yield return new TestCaseData(Rows3X3.CounterDiagonal);
            }
        } 
    }
}
