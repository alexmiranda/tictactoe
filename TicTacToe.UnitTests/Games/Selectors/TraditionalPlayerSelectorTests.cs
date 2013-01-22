using NUnit.Framework;
using TicTacToe.Games;
using TicTacToe.Games.Selectors;

namespace TicTacToe.UnitTests.Games.Selectors
{
    [TestFixture]
    public class TraditionalPlayerSelectorTests
    {
        private TraditionalPlayerSelector _selector;

        [SetUp]
        public void SetUp()
        {
            _selector = new TraditionalPlayerSelector();
        }

        [Test]
        public void TheFirstPlayerIsAlwaysCross()
        {
            Assert.That(_selector.Next(), Is.EqualTo(Mark.Cross));
        }

        [Test]
        public void SwitchesFromOnePlayerToAnother()
        {
            Assert.That(_selector.Next(), Is.EqualTo(Mark.Cross));
            Assert.That(_selector.Next(), Is.EqualTo(Mark.Nought));
        }
    }
}
