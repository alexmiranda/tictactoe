using NUnit.Framework;
using System;
using System.Collections.Generic;
using TicTacToe.Games;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;
using TicTacToe.UnitTests.Support.Data;

namespace TicTacToe.UnitTests.Games.Grid
{
    [TestFixture]
    public class Grid3X3Tests
    {
        private IGrid _grid;

        [SetUp]
        public void InitialiseFixture()
        {
            _grid = new Grid3X3();
        }

        [Test]
        public void IsBlankDoesNotAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => _grid.IsBlank(null));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Positions")]
        public void AllPositionsShouldBeBlankWhenANewGridIsCreated(Positions3X3 position)
        {
            Assert.That(_grid.IsBlank(position), Is.True);
        }

        [Test]
        public void IsFilledDoesNotAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => _grid.IsFilled(null));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Positions")]
        public void AllPositionsShouldNotBeFilledWhenANewGridIsCreated(Positions3X3 position)
        {
            Assert.That(_grid.IsFilled(position), Is.False);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void ItShouldNotAcceptMarkNull(Mark mark)
        {
            Assert.Throws<ArgumentNullException>(() => _grid.Fill(null, mark));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Positions")]
        public void WhenIMarkAPosition_ItShouldNotBeBlank(Positions3X3 position)
        {
            _grid = Mark.Cross.On(_grid, position);
            Assert.That(_grid.IsBlank(position), Is.False);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Positions")]
        public void WhenIMarkAPosition_ItShouldNotBeFilled(Positions3X3 position)
        {
            _grid = Mark.Cross.On(_grid, position);
            Assert.That(_grid.IsFilled(position), Is.True);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Rows")]
        public void CompletedRowsReturnsCorrectly(IRow row)
        {
            foreach (var position in row.Positions)
                _grid = _grid.Fill(position, Mark.Cross);
            var completedRows = _grid.CompletedRows();
            Assert.That(completedRows, Is.Not.Null);
            Assert.That(completedRows, Contains.Item(row));
        }

        public IEnumerable<Positions3X3> AllPositions
        {
            get 
            { 
                yield return Positions3X3.TopLeftCorner;
                yield return Positions3X3.TopRightCorner;
                yield return Positions3X3.BottomLeftCorner;
                yield return Positions3X3.BottomRightCorner;
                yield return Positions3X3.Center;
                yield return Positions3X3.LeftEdge;
                yield return Positions3X3.TopEdge;
                yield return Positions3X3.RightEdge;
                yield return Positions3X3.BottomEdge;
            }
        }
    }
}
