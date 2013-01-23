using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using TicTacToe.Games;
using TicTacToe.Player;

namespace TicTacToe.UnitTests.Player
{
    [TestFixture]
    public class ComputerPlayerTests
    {
        private const string PlayerName = "Player's Name";

        [Test]
        public void PlayersNameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ComputerPlayer(null, Mark.Cross));
        }

        [Test]
        public void PlayersNameCannotBeEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new ComputerPlayer(string.Empty, Mark.Cross));
        }

        [Test]
        public void PlayerNameShouldBeSetCorrectly()
        {
            var player = new ComputerPlayer(PlayerName, Mark.Cross);
            Assert.That(player.Name, Is.EqualTo(PlayerName));
        }

        [Test]
        public void MarkCannotBeNull()
        {
            Assert.Throws<ArgumentException>(() => new ComputerPlayer(PlayerName, Mark.Blank));
        }

        [Test]
        public void MarkShouldBeSetCorrectly()
        {
            var player = new ComputerPlayer(PlayerName, Mark.Cross);
            Assert.That(player.Mark, Is.EqualTo(Mark.Cross));
        }

        [Test]
        public void JoinGameShouldSubscribeItselfToTheGame()
        {
            var game = new Mock<IGame>();
            game.Setup(x => x.Subscribe(It.IsAny<IPlayer>())).Verifiable();

            var player = new ComputerPlayer(PlayerName, Mark.Cross);
            player.Join(game.Object);

            game.Verify(x => x.Subscribe(player), Times.Once());
        }

        [Test]
        public void DisposeShouldLeaveTheGame()
        {
            var trigger = new ManualResetEvent(false);

            var ticket = new Mock<IDisposable>();
            ticket.Setup(x => x.Dispose())
                .Callback(() => trigger.Set());

            var game = new Mock<IGame>();
            game.Setup(x => x.Subscribe(It.IsAny<IPlayer>()))
                .Returns(ticket.Object);

            using (var player = new ComputerPlayer(PlayerName, Mark.Cross))
            {
                player.Join(game.Object);
            }

            trigger.WaitOne();

            ticket.Verify(x => x.Dispose(), Times.Once());
        }
    }
}
