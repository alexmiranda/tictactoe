using Moq;
using NUnit.Framework;
using System;
using TicTacToe.Games;
using TicTacToe.Games.Grid;
using TicTacToe.UnitTests.Support.Data;

namespace TicTacToe.UnitTests.Games
{
    [TestFixture]
    public class MoveTests
    {
        private IPosition _position;

        [SetUp]
        public void SetUp()
        {
            _position = new Mock<IPosition>().Object;
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void DoesNotAcceptNullPosition(Mark mark)
        {
            Assert.Throws<ArgumentNullException>(() => new Move(mark, null));
        }

        [Test]
        public void DoesNotAcceptBlankMark()
        {
            Assert.Throws<ArgumentException>(() => new Move(Mark.Blank, _position));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void CanCreateAMoveWithMarkAndPosition(Mark mark)
        {
            Assert.DoesNotThrow(() => new Move(mark, _position));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void MovesAreEqual(Mark mark)
        {
            var oneMove = new Move(mark, _position);
            var anotherOne = new Move(mark, _position);
            Assert.That(oneMove, Is.EqualTo(anotherOne));
        }
    }
}
