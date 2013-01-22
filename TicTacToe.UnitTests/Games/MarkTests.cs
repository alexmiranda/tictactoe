using Moq;
using NUnit.Framework;
using System;
using TicTacToe.Games;
using TicTacToe.Games.Grid;
using TicTacToe.UnitTests.Support.Data;

namespace TicTacToe.UnitTests.Games
{
    [TestFixture]
    public class MarkTests
    {
        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Positions")]
        public void WhenMarkPosition_TheGridShouldGetUpdatedForTheSamePosition(IPosition position)
        {
            var mock = new Mock<IGrid<IPosition>>();
            mock.SetupSet(x => x[position] = Mark.Cross);
            Mark.Cross.On(mock.Object, position);
            mock.Verify();
        }

        [Test]
        public void SwitchMarkFromCrossReturnsNought()
        {
            Assert.That(Mark.Cross.Switch(), Is.EqualTo(Mark.Nought));
        }

        [Test]
        public void SwitchMarkFromNoughtReturnsCross()
        {
            Assert.That(Mark.Nought.Switch(), Is.EqualTo(Mark.Cross));
        }

        [Test]
        public void SwitchMarkFromBlankRaisesAnException()
        {
            Assert.Throws<InvalidOperationException>(() => Mark.Blank.Switch());
        }
    }
}
