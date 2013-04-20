using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TicTacToe.Games;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;
using TicTacToe.Games.Rules;
using TicTacToe.UnitTests.Support.Data;
using TicTacToe.UnitTests.Support.Fakes;

namespace TicTacToe.UnitTests.Games
{
    [TestFixture]
    public class GameTests
    {
        private Game _game;
        private FakeGrid _fakeGrid;
        private FakePosition _invalidPosition;

        private IList<Mock<IObserver<IGame>>> _observers;
        private Mock<IObserver<IGame>> _observer;
        private Mock<IObserver<IGame>> _observer2;

        private CountdownEvent _trigger;
        private CountdownEvent _triggerCompleted;
        private CountdownEvent _triggerError;

        [SetUp]
        public void SetUp()
        {
            _fakeGrid = new FakeGrid();
            _game = new Game(_fakeGrid);
            _invalidPosition = new FakePosition();
            SetupObservers();
        }

        [TearDown]
        public void TearDown()
        {
            _trigger.Dispose();
            _triggerCompleted.Dispose();
            _triggerError.Dispose();
        }

        [Test]
        public void EnsureGridIsNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Game(null));
        }

        [Test]
        public void WhenCreateNewGame_GridIsNotChanged()
        {
            Assert.That(_game.Grid, Is.SameAs(_fakeGrid));
        }

        [Test]
        public void WhenCreateNewGame_StatusIsNew()
        {
            Assert.That(_game.Status, Is.SameAs(GameStatus.New));
        }

        [Test]
        public void WhenStartGame_StatusIsStarted()
        {
            _game.Start();
            Assert.That(_game.Status.IsStarted());
        }

        [Test]
        public void ObserverCanSubscribe_AfterStartGame()
        {
            Assert.DoesNotThrow(() =>
                                    {
                                        _game.Start();
                                        _game.Subscribe(_observer.Object);
                                    });
        }

        [Test]
        public void ObserverCanSubscribeTwice()
        {
            Assert.DoesNotThrow(() =>
                                    {
                                        _game.Subscribe(_observer.Object);
                                        _game.Subscribe(_observer.Object);
                                    });
        }

        [Test]
        public void ObserverCanSubscribeTwice_AfterStartGame()
        {
            Assert.DoesNotThrow(() =>
                                    {
                                        _game.Start();
                                        _game.Subscribe(_observer.Object);
                                        _game.Subscribe(_observer.Object);
                                    });
        }

        [Test]
        public void WhenStartGame_StatusIsOver_IfGridHasSomeCompletedRow()
        {
            ForceGameOver();
            _game.Start();
            Assert.That(_game.Status.IsOver());
        }

        [Test]
        public void WhenStartGame_ObserversAreNotified()
        {
            _game.Start();
            WaitUntilObserversAreNotified();
            VerifyObserversAreNotified();
        }

        [Test]
        public void WhenStartGame_ObserversAreNotifiedCompleted_IfGameIsOver()
        {
            ForceGameOver();
            _game.Start();

            WaitUntilObserversAreNotifiedCompleted();
            VerifyObserversAreNotifiedCompleted();
        }

        [Test]
        public void GameCannotBeStartedAgain()
        {
            Assert.Throws<StatusChangeException>(() =>
                                                     {
                                                         _game.Start();
                                                         _game.Start();
                                                     });
        }

        [Test]
        public void GameCannotBeStarted_EvenIfItIsOver()
        {
            Assert.Throws<StatusChangeException>(() =>
                                                     {
                                                         ForceGameOver();
                                                         _game.Start();
                                                         _game.Start();
                                                     });
        }

        [Test]
        public void WhenStartGame_RuleAssistantIsInitialised()
        {
            var assistant = new Mock<IRuleAssistant>();
            assistant.Setup(x => x.SetGame(It.IsAny<IGame>()));

            _game.Start(assistant.Object);

            assistant.Verify(x => x.SetGame(_game), Times.Once());
        }

        [Test]
        public void CannotPlayBeforeStartGame()
        {
            Assert.Throws<GameNotStartedException>(() =>
                                                       {
                                                           Assert.That(_game.Status.IsStarted(), Is.False);
                                                           _game.Play(Mark.Cross, Positions3X3.Center);
                                                       });
        }

        [Test]
        public void CannotPlay_IfGameIsOver_JustAfterStartIt()
        {
            Assert.Throws<GameNotStartedException>(() =>
                                                       {
                                                           ForceGameOver();
                                                           _game.Start();

                                                           Assert.That(_game.Status.IsOver());

                                                           _game.Play(Mark.Cross, Positions3X3.Center);
                                                       });
        }

        [Test]
        [TestCaseSource(typeof (TestCaseDataProvider), "Marks")]
        public void AnyPlayerCanPlayTheFirstMove(Mark mark)
        {
            Assert.DoesNotThrow(() =>
                                    {
                                        _game.Start();
                                        _game.Play(mark, Positions3X3.Center);
                                    });
        }

        [Test]
        public void CannotPlayBlank()
        {
            Assert.Throws<BadMoveException>(() =>
                                                {
                                                    _game.Start();
                                                    _game.Play(Mark.Blank, Positions3X3.Center);
                                                });
        }

        [Test]
        [TestCaseSource(typeof (TestCaseDataProvider), "Marks")]
        public void EnsureDoesNotAcceptInvalidPositions(Mark mark)
        {
            Assert.Throws<ArgumentException>(() =>
                                                 {
                                                     _game.Start();
                                                     _game.Play(mark, _invalidPosition);
                                                 });
        }

        [Test]
        public void EnsureSamePlayerCannotPlayTwiceInSequence()
        {
            Assert.Throws<GameSequenceException>(() =>
                                                     {
                                                         _game.Start();
                                                         _game.Play(Mark.Cross, Positions3X3.Center);
                                                         _game.Play(Mark.Cross, Positions3X3.TopLeftCorner);
                                                     });
        }

        [Test]
        public void EnsureDifferentPlayerPlaysSecondMove()
        {
            Assert.DoesNotThrow(() =>
            {
                _game.Start();
                _game.Play(Mark.Cross, Positions3X3.Center);
                _game.Play(Mark.Nought, Positions3X3.TopLeftCorner);
            });
        }

        [Test]
        public void EnsureCannotPlayOnFilledPosition()
        {
            Assert.Throws<FilledPositionException>(() =>
            {
                Mark.Cross.On(_fakeGrid, Positions3X3.Center);
                _game.Start(); 
                _game.Play(Mark.Nought, Positions3X3.Center);
            });
        }

        [Test]
        public void EnsureAssistantAcceptsMoveBeforePlaying()
        {
            var assistant = new Mock<IRuleAssistant>();
            assistant.Setup(x => x.AcceptMove(It.IsAny<Move>()));

            _game.Start(assistant.Object);
            _game.Play(Mark.Cross, Positions3X3.Center);

            assistant.Verify(x => x.AcceptMove(It.IsAny<Move>()), Times.Once());
        }

        [Test]
        public void EnsureCreatesMoveProperly()
        {
            Move move = null;
            var assistant = new Mock<IRuleAssistant>();
            assistant.Setup(x => x.AcceptMove(It.IsAny<Move>()))
                .Callback((Move m) => move = m);

            _game.Start(assistant.Object);
            _game.Play(Mark.Cross, Positions3X3.Center);

            Assert.That(move, Is.Not.Null);
            Assert.That(move.Mark, Is.EqualTo(Mark.Cross));
            Assert.That(move.Position, Is.EqualTo(Positions3X3.Center));
        }

        [Test]
        public void GameIsOverIfThereAreAnyCompletedRow()
        {
            IGrid grid = new Grid3X3();
            grid = grid.Fill(Positions3X3.Center, Mark.Cross)
                .Fill(Positions3X3.BottomLeftCorner, Mark.Cross);

            _game = new Game(grid);
            _game.Start();

            _game.Play(Mark.Cross, Positions3X3.TopRightCorner);
            Assert.That(_game.Status.IsOver());
        }

        private void SetupObservers()
        {
            AddObservers();

            var initialCount = _observers.Count;
            _trigger = new CountdownEvent(initialCount);
            _triggerCompleted = new CountdownEvent(initialCount);
            _triggerError = new CountdownEvent(initialCount);

            foreach (var observer in _observers)
            {
                observer.Setup(x => x.OnNext(It.IsAny<IGame>()))
                    .Callback(() => _trigger.Signal());

                observer.Setup(x => x.OnCompleted())
                    .Callback(() => _triggerCompleted.Signal());

                observer.Setup(x => x.OnError(It.IsAny<Exception>()))
                    .Callback(() => _triggerError.Signal());

                _game.Subscribe(observer.Object);
            }
        }

        private void AddObservers()
        {
            _observer = new Mock<IObserver<IGame>>();
            _observer2 = new Mock<IObserver<IGame>>();
            _observers = new List<Mock<IObserver<IGame>>>(2) { _observer, _observer2 };
        }

        private void WaitUntilObserversAreNotified(int seconds = 1)
        {
            _trigger.Wait(TimeSpan.FromSeconds(seconds));
        }

        private void WaitUntilObserversAreNotifiedCompleted(int seconds = 1)
        {
            _triggerCompleted.Wait(TimeSpan.FromSeconds(seconds));
        }

        private void WaitUntilObserversAreNotifiedError(int seconds = 1)
        {
            _triggerError.Wait(TimeSpan.FromSeconds(seconds));
        }

        private void VerifyObserversAreNotified()
        {
            foreach (var observer in _observers)
            {
                observer.Verify(x => x.OnNext(_game), Times.Once());
            }
        }

        private void VerifyObserversAreNotifiedCompleted()
        {
            foreach (var observer in _observers)
            {
                observer.Verify(x => x.OnCompleted(), Times.Once());
            }
        }

        private void VerifyObserversAreNotifiedError<TException>() where TException : Exception
        {
            VerifyObserversAreNotifiedError(It.IsAny<TException>());
        }

        private void VerifyObserversAreNotifiedError(Exception e)
        {
            foreach (var observer in _observers)
            {
                observer.Verify(x => x.OnError(e), Times.Once());
            }
        }

        private IEnumerable<IRow> ForceGameOver(params IRow[] rows)
        {
            var completedRows = rows.ToList();
            if (!completedRows.Any())
                completedRows.Add(null);
            _fakeGrid.SetCompletedRows(completedRows);
            return completedRows;
        }
    }

    //[TestFixture]
    //public class OldGameTests
    //{
    //    private Game _game;
    //    private Mock<Grid3X3> _mockGrid;
    //    private Mock<IObserver<IGame>> _mockObserver;
    //    private Positions3X3 _position;
    //    private Positions3X3 _position2;
    //    private Mock<IRuleAssistant> _mockAssistant;

    //    [SetUp]
    //    public void StartNewGame()
    //    {
    //        _mockGrid = new Mock<Grid3X3>();
    //        _mockObserver = new Mock<IObserver<IGame>>();
    //        _position = Positions3X3.Center;
    //        _position2 = Positions3X3.LeftEdge;
    //        _mockAssistant = new Mock<IRuleAssistant>();
    //        _game = NewGame();
    //        _game.Start();
    //    }

    //    [Test]
    //    public void GridMustBeProvided()
    //    {
    //        Assert.Throws<ArgumentNullException>(() => new Game(null));
    //    }

    //    [Test]
    //    public void WhenCreateANewGame_StatusIsNew()
    //    {
    //        _game = NewGame();
    //        Assert.That(_game.Status, Is.Not.Null);
    //        Assert.That(_game.Status, Is.EqualTo(GameStatus.New));
    //    }

    //    [Test]
    //    public void CannotPlayBeforeStartTheGame()
    //    {
    //        Assert.Throws<GameNotStartedException>(() =>
    //                                                   {
    //                                                       _game = NewGame();
    //                                                       _game.Play(Mark.Cross, _position);
    //                                                   });
    //    }

    //    [Test]
    //    public void WhenStartTheGame_StatusIsStarted()
    //    {
    //        _game = NewGame();
    //        _game.Start();
    //        Assert.That(_game.Status, Is.Not.Null);
    //        Assert.That(_game.Status.IsStarted(), Is.Not.Null);
    //    }

    //    [Test]
    //    public void WhenStartTheGame_AssistantIsInitialised()
    //    {
    //        _mockAssistant.Setup(x => x.SetGame(It.IsAny<IGame>())).Verifiable();
    //        _game = NewGame();
    //        _game.Start(_mockAssistant.Object);
    //        _mockAssistant.Verify(x => x.SetGame(_game), Times.Once());
    //    }

    //    [Test]
    //    public void WhenStartTheGame_MovesIsEmpty()
    //    {
    //        _game = NewGame();
    //        Assert.That(_game.Moves, Is.Not.Null);
    //        Assert.That(_game.Moves, Is.Empty);
    //    }

    //    [Test]
    //    [TestCaseSource(typeof (TestCaseDataProvider), "Marks")]
    //    public void TheFirstMoveCanBeMadeByAnyPlayer(Mark mark)
    //    {
    //        _game.Play(mark, _position);
    //    }

    //    [Test]
    //    [TestCaseSource(typeof (TestCaseDataProvider), "Marks")]
    //    public void TheSecondMoveCannotBeMadeByTheSamePlayer(Mark mark)
    //    {
    //        Assert.Throws<GameSequenceException>(() =>
    //                                                 {
    //                                                     _game.Play(mark, _position);
    //                                                     _game.Play(mark, _position2);
    //                                                 });
    //    }

    //    [Test]
    //    public void ItShouldNotAcceptPlayingBlank()
    //    {
    //        Assert.Throws<BadMoveException>(() => _game.Play(Mark.Blank, _position));
    //    }

    //    [Test]
    //    public void ItShouldOnlyAccept3x3Positions()
    //    {
    //        Assert.Throws<ArgumentException>(() => _game.Play(Mark.Cross, new FakePosition()));
    //    }

    //    [Test]
    //    [TestCaseSource(typeof (TestCaseDataProvider), "Marks")]
    //    public void ItShouldNotAcceptNullForPosition(Mark mark)
    //    {
    //        Assert.Throws<ArgumentNullException>(() => _game.Play(mark, null));
    //    }

    //    [Test]
    //    public void ItShouldNotAcceptPlayingOnAFilledPosition()
    //    {
    //        _mockGrid.Setup(x => x.IsFilled(It.IsAny<Positions3X3>())).Returns(true);
    //        Assert.Throws<FilledPositionException>(() => _game.Play(Mark.Cross, Positions3X3.Center));
    //        _mockGrid.Verify();
    //    }

    //    [Test]
    //    public void ShouldMarkThePositionOnInternalGrid()
    //    {
    //        _mockGrid.Setup(x => x.Fill(_position, Mark.Cross))
    //            .Verifiable("Fail to mark on the grid");
    //        _game.Play(Mark.Cross, _position);
    //        _mockGrid.Verify(x => x.Fill(_position, Mark.Cross), Times.Once());
    //    }

    //    [Test]
    //    [ExpectedException]
    //    public void SubscribeShouldNotAcceptNull()
    //    {
    //        _game.Subscribe(null);
    //    }

    //    [Test]
    //    public void ShouldNotifyPlayersWhenAMoveIsAccepted()
    //    {
    //        var trigger = new ManualResetEvent(false);
    //        _mockObserver.Setup(x => x.OnNext(_game))
    //            .Callback(() => trigger.Set())
    //            .Verifiable("Fail to notify the observers");
    //        _game.Subscribe(_mockObserver.Object);
    //        _game.Play(Mark.Cross, _position);
    //        trigger.WaitOne();
    //        _mockObserver.Verify(x => x.OnNext(_game), Times.Once());
    //    }

    //    [Test]
    //    public void AnyPlayerCanJoinTheGame()
    //    {
    //        Assert.DoesNotThrow(() =>
    //                                {
    //                                    var player = new Mock<IPlayer>().Object;
    //                                    _game.Subscribe(player);
    //                                });
    //    }

    //    [Test]
    //    public void JoinTwice_DoesNotCauseAnyError()
    //    {
    //        Assert.DoesNotThrow(() =>
    //                                {
    //                                    var player = new Mock<IPlayer>().Object;
    //                                    _game.Subscribe(player);
    //                                    _game.Subscribe(player);
    //                                });
    //    }

    //    [Test]
    //    public void WhenAMoveIsAccepted_ItShouldBeAddedOnLastMoves()
    //    {
    //        var move = new Move(Mark.Cross, _position);
    //        _game.Play(Mark.Cross, _position);
    //        var lastMove = _game.Moves.LastOrDefault();
    //        Assert.That(lastMove, Is.Not.Null);
    //        Assert.That(move, Is.EqualTo(lastMove));
    //    }

    //    [Test]
    //    public void AssistantShouldBeAskedBeforeAMoveIsAccepted()
    //    {
    //        _mockAssistant.Setup(x => x.AcceptMove(It.IsAny<Move>())).Verifiable();
    //        _game = NewGame();
    //        _game.Start(_mockAssistant.Object);
    //        _game.Play(Mark.Cross, _position);
    //        _mockAssistant.Verify(x => x.AcceptMove(It.IsAny<Move>()), Times.Once());
    //    }

    //    [Test]
    //    public void IfAssistantDoesNotAcceptAMove_AllObserversShouldBeNotified()
    //    {
    //        var trigger = new ManualResetEvent(false);

    //        _mockAssistant.Setup(x => x.AcceptMove(It.IsAny<Move>()))
    //            .Throws<Exception>();

    //        var observer = new Mock<IObserver<IGame>>();
    //        observer.Setup(x => x.OnError(It.IsAny<Exception>()))
    //            .Callback(() => trigger.Set());

    //        _game = NewGame();
    //        _game.Start(_mockAssistant.Object);
    //        _game.Subscribe(observer.Object);

    //        Assert.Throws<Exception>(() => _game.Play(Mark.Cross, _position));

    //        trigger.WaitOne(1000);

    //        _mockAssistant.Verify(x => x.AcceptMove(It.IsAny<Move>()), Times.Once());
    //        observer.Verify(x => x.OnError(It.IsAny<Exception>()), Times.Once());
    //    }

    //    [Test]
    //    [TestCaseSource(typeof (TestCaseDataProvider), "Rows")]
    //    public void IfAtTheStartOfTheGame_ThereAreCompletedRows_ItShouldFinishAndNotify(IRow row)
    //    {
    //        var trigger = new ManualResetEvent(false);

    //        var mockPlayer = new Mock<IPlayer>();
    //        mockPlayer.Setup(x => x.OnCompleted())
    //            .Callback(() => trigger.Set());

    //        var mockGrid = new Mock<Grid3X3>();
    //        mockGrid.Setup(x => x.Get(It.IsAny<IPosition>()))
    //            .Returns((IPosition position) => row.Includes(position) ? Mark.Cross : Mark.Blank);

    //        _game = new Game(mockGrid.Object);
    //        _game.Subscribe(mockPlayer.Object);

    //        _game.Start();

    //        Assert.That(_game.Status.IsOver());

    //        trigger.WaitOne(1000);

    //        mockPlayer.Verify(x => x.OnCompleted(), Times.Once());
    //    }

    //    [Test]
    //    public void ShouldCheckIfTheGameIsOver()
    //    {
    //        var fakeListOfCompletedRows = new List<IRow>();

    //        var mockGrid = new Mock<Grid3X3>();
    //        mockGrid.Setup(x => x.Get(It.IsAny<IPosition>())).Returns(Mark.Cross);

    //        _game = new Game(mockGrid.Object);
    //        _game.Start();

    //        fakeListOfCompletedRows.Add(Rows3X3.CounterDiagonal);

    //        _game.Play(Mark.Cross, Positions3X3.TopLeftCorner);

    //        Assert.That(_game.Status.IsOver());
    //    }

    //    private Game NewGame()
    //    {
    //        return new Game(_mockGrid.Object);
    //    }
    //}
}
