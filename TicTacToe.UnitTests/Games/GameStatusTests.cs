using NUnit.Framework;
using TicTacToe.Games;

namespace TicTacToe.UnitTests.Games
{
    [TestFixture]
    public class GameStatusTests
    {
        private GameStatus _status;

        [SetUp]
        public void SetUp()
        {
            _status = GameStatus.New;
        }

        [Test]
        public void GameStatus_New_IsNotStarted_And_NotOver()
        {
            Assert.That(_status.IsStarted(), Is.False);
            Assert.That(_status.IsOver(), Is.False);
            Assert.That(_status.ToString(), Is.EqualTo("New"));
        }

        [Test]
        public void CanChangeFromNewToStarted()
        {
            _status = _status.ToStarted();
            Assert.That(_status.IsStarted(), Is.True);
            Assert.That(_status.IsOver(), Is.False);
            Assert.That(_status.ToString(), Is.EqualTo("Started"));
        }

        [Test]
        public void CannotChangeFromNewToOver()
        {
            Assert.Throws<StatusChangeException>(() => _status.ToOver());
        }

        [Test]
        public void CanChangeFromNewToStarted_AndThen_ToOver()
        {
            _status = _status.ToStarted().ToOver();
            Assert.That(_status.IsStarted(), Is.False);
            Assert.That(_status.IsOver(), Is.True);
            Assert.That(_status.ToString(), Is.EqualTo("Over"));
        }

        [Test]
        public void CannotChangeFromNewToStartedThenToOver_AndThen_ToStartedAgain()
        {
            Assert.Throws<StatusChangeException>(() => _status = _status.ToStarted().ToOver().ToStarted());
        }
    }
}
