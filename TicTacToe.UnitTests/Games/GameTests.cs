using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TicTacToe.Games;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;
using TicTacToe.Games.Rules;
using TicTacToe.Player;
using TicTacToe.UnitTests.Support.Data;
using TicTacToe.UnitTests.Support.Fakes;

namespace TicTacToe.UnitTests.Games
{
    [TestFixture]
    public class GameTests
    {
        private Game _game;
        private Mock<Grid3X3> _mockGrid;
        private Mock<IObserver<IGame>> _mockObserver;
        private Positions3X3 _position;
        private Positions3X3 _position2;
        private Mock<IRuleAssistant> _mockAssistant;

        [SetUp]
        public void StartNewGame()
        {
            _mockGrid = new Mock<Grid3X3>();
            _mockObserver = new Mock<IObserver<IGame>>();
            _position = Positions3X3.Center;
            _position2 = Positions3X3.LeftEdge;
            _mockAssistant = new Mock<IRuleAssistant>();
            _game = NewGame();
            _game.Start();
        }

        [Test]
        public void GridMustBeProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new Game(null));
        }

        [Test]
        public void WhenCreateANewGame_StatusIsNew()
        {
            _game = NewGame();
            Assert.That(_game.Status, Is.Not.Null);
            Assert.That(_game.Status, Is.EqualTo(GameStatus.New));
        }

        [Test]
        public void CannotPlayBeforeStartTheGame()
        {
            Assert.Throws<GameNotStartedException>(() =>
            {
                _game = NewGame();
                _game.Play(Mark.Cross, _position);
            });
        }

        [Test]
        public void WhenStartTheGame_StatusIsStarted()
        {
            _game = NewGame();
            _game.Start();
            Assert.That(_game.Status, Is.Not.Null);
            Assert.That(_game.Status.IsStarted(), Is.Not.Null);
        }

        [Test]
        public void WhenStartTheGame_AssistantIsInitialised()
        {
            _mockAssistant.Setup(x => x.SetGame(It.IsAny<IGame>())).Verifiable();
            _game = NewGame();
            _game.Start(_mockAssistant.Object);
            _mockAssistant.Verify(x => x.SetGame(_game), Times.Once());
        }

        [Test]
        public void WhenStartTheGame_MovesIsEmpty()
        {
            _game = NewGame();
            Assert.That(_game.Moves, Is.Not.Null);
            Assert.That(_game.Moves, Is.Empty);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void TheFirstMoveCanBeMadeByAnyPlayer(Mark mark)
        {
            _game.Play(mark, _position);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void TheSecondMoveCannotBeMadeByTheSamePlayer(Mark mark)
        {
            Assert.Throws<GameSequenceException>(() =>
            {
                _game.Play(mark, _position);
                _game.Play(mark, _position2);
            });
        }

        [Test]
        public void ItShouldNotAcceptPlayingBlank()
        {
            Assert.Throws<BadMoveException>(() => _game.Play(Mark.Blank, _position));
        }

        [Test]
        public void ItShouldOnlyAccept3x3Positions()
        {
            Assert.Throws<ArgumentException>(() => _game.Play(Mark.Cross, new FakePosition()));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Marks")]
        public void ItShouldNotAcceptNullForPosition(Mark mark)
        {
            Assert.Throws<ArgumentNullException>(() => _game.Play(mark, null));
        }

        [Test]
        public void ItShouldNotAcceptPlayingOnAFilledPosition()
        {
            _mockGrid.Setup(x => x.IsFilled(It.IsAny<Positions3X3>())).Returns(true);
            Assert.Throws<FilledPositionException>(() => _game.Play(Mark.Cross, Positions3X3.Center));
            _mockGrid.Verify();
        }

        [Test]
        public void ShouldMarkThePositionOnInternalGrid()
        {
            _mockGrid.SetupSet(x => x[_position] = Mark.Cross)
                .Verifiable("Fail to mark on the grid");
            _game.Play(Mark.Cross, _position);
            _mockGrid.VerifySet(x => x[_position] = Mark.Cross, Times.Once());
        }

        [Test]
        [ExpectedException]
        public void SubscribeShouldNotAcceptNull()
        {
            _game.Subscribe(null);
        }

        [Test]
        public void ShouldNotifyPlayersWhenAMoveIsAccepted()
        {
            var trigger = new ManualResetEvent(false);
            _mockObserver.Setup(x => x.OnNext(_game))
                .Callback(() => trigger.Set())
                .Verifiable("Fail to notify the observers");
            _game.Subscribe(_mockObserver.Object);
            _game.Play(Mark.Cross, _position);
            trigger.WaitOne();
            _mockObserver.Verify(x => x.OnNext(_game), Times.Once());
        }

        [Test]
        public void AnyPlayerCanJoinTheGame()
        {
            Assert.DoesNotThrow(() => {
                var player = new Mock<IPlayer>().Object;
                _game.Subscribe(player);
            });
        }

        [Test]
        public void JoinTwice_DoesNotCauseAnyError()
        {
            Assert.DoesNotThrow(() => {
                var player = new Mock<IPlayer>().Object;
                _game.Subscribe(player);
                _game.Subscribe(player);
            });
        }

        [Test]
        public void WhenAMoveIsAccepted_ItShouldBeAddedOnLastMoves()
        {
            var move = new Move(Mark.Cross, _position);
            _game.Play(Mark.Cross, _position);
            var lastMove = _game.Moves.LastOrDefault();
            Assert.That(lastMove, Is.Not.Null);
            Assert.That(move, Is.EqualTo(lastMove));
        }

        [Test]
        public void AssistantShouldBeAskedBeforeAMoveIsAccepted()
        {
            _mockAssistant.Setup(x => x.AcceptMove(It.IsAny<Move>())).Verifiable();
            _game = NewGame();
            _game.Start(_mockAssistant.Object);
            _game.Play(Mark.Cross, _position);
            _mockAssistant.Verify(x => x.AcceptMove(It.IsAny<Move>()), Times.Once());
        }

        [Test]
        public void IfAssistantDoesNotAcceptAMove_AllObserversShouldBeNotified()
        {
            var trigger = new ManualResetEvent(false);

            _mockAssistant.Setup(x => x.AcceptMove(It.IsAny<Move>()))
                .Throws<Exception>();

            var observer = new Mock<IObserver<IGame>>(); 
            observer.Setup(x => x.OnError(It.IsAny<Exception>()))
                .Callback(() => trigger.Set());

            _game = NewGame();
            _game.Start(_mockAssistant.Object);
            _game.Subscribe(observer.Object);
            
            Assert.Throws<Exception>(() => _game.Play(Mark.Cross, _position));

            trigger.WaitOne(1000);

            _mockAssistant.Verify(x => x.AcceptMove(It.IsAny<Move>()), Times.Once());
            observer.Verify(x => x.OnError(It.IsAny<Exception>()), Times.Once());
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Rows")]
        public void IfAtTheStartOfTheGame_ThereAreCompletedRows_ItShouldFinishAndNotify(IRow row)
        {
            var trigger = new ManualResetEvent(false);
            
            var mockPlayer = new Mock<IPlayer>();
            mockPlayer.Setup(x => x.OnCompleted())
                .Callback(() => trigger.Set());

            var mockGrid = new Mock<Grid3X3>();
            mockGrid.Setup(x => x.CompletedRows())
                .Returns(new List<IRow>() { row })
                .Verifiable("CompletedRows was not called");

            _game = new Game(mockGrid.Object);
            _game.Subscribe(mockPlayer.Object);

            _game.Start();

            Assert.That(_game.Status.IsOver());

            trigger.WaitOne(1000);

            mockGrid.Verify(x => x.CompletedRows(), Times.Once());
            mockPlayer.Verify(x => x.OnCompleted(), Times.Once());
        }

        [Test]
        public void ShouldCheckIfTheGameIsOver()
        {
            var fakeListOfCompletedRows = new List<IRow>();

            var mockGrid = new Mock<Grid3X3>();
            mockGrid.Setup(x => x.CompletedRows())
                .Returns(fakeListOfCompletedRows)
                .Verifiable();

            _game = new Game(mockGrid.Object);
            _game.Start();

            fakeListOfCompletedRows.Add(Rows3X3.CounterDiagonal);

            _game.Play(Mark.Cross, Positions3X3.TopLeftCorner);

            Assert.That(_game.Status.IsOver());
            mockGrid.Verify(x => x.CompletedRows(), Times.Exactly(2));
        }

        private Game NewGame()
        {
            return new Game(_mockGrid.Object);
        }
    }
}
