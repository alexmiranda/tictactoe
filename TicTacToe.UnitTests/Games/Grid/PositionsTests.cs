using System;
using NUnit.Framework;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;

namespace TicTacToe.UnitTests.Games.Grid
{
    [TestFixture]
    public class PositionsTests
    {
        [Test]
        public void ShouldBeAbleToGetPositionsByName()
        {
            var topLeftCorner = Positions.GetByName<Positions3X3>("TopLeftCorner");
            Assert.That(topLeftCorner, Is.EqualTo(Positions3X3.TopLeftCorner));

            var topRightCorner = Positions.GetByName<Positions3X3>("TopRightCorner");
            Assert.That(topRightCorner, Is.EqualTo(Positions3X3.TopRightCorner));

            var bottomLeftCorner = Positions.GetByName<Positions3X3>("BottomLeftCorner");
            Assert.That(bottomLeftCorner, Is.EqualTo(Positions3X3.BottomLeftCorner));

            var bottomRightCorner = Positions.GetByName<Positions3X3>("BottomRightCorner");
            Assert.That(bottomRightCorner, Is.EqualTo(Positions3X3.BottomRightCorner));

            var center = Positions.GetByName<Positions3X3>("Center");
            Assert.That(center, Is.EqualTo(Positions3X3.Center));

            var leftEdge = Positions.GetByName<Positions3X3>("LeftEdge");
            Assert.That(leftEdge, Is.EqualTo(Positions3X3.LeftEdge));

            var topEdge = Positions.GetByName<Positions3X3>("TopEdge");
            Assert.That(topEdge, Is.EqualTo(Positions3X3.TopEdge));

            var rightEdge = Positions.GetByName<Positions3X3>("RightEdge");
            Assert.That(rightEdge, Is.EqualTo(Positions3X3.RightEdge));

            var bottomEdge = Positions.GetByName<Positions3X3>("BottomEdge");
            Assert.That(bottomEdge, Is.EqualTo(Positions3X3.BottomEdge));
        }

        [Test]
        public void ShouldThrowExceptionIfThePositionNameDoesNotExist()
        {
            Assert.Throws<ArgumentException>(() => Positions.GetByName<Positions3X3>("DoesNotExist"));
        }
    }
}
