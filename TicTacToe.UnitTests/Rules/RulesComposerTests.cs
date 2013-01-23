using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicTacToe.Games;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;
using TicTacToe.Games.Rules;

namespace TicTacToe.UnitTests.Rules
{
    [TestFixture]
    public class RulesComposerTests
    {
        private RulesComposer _composer;
        private Mock<IGame> _gameMock;

        [SetUp]
        public void CanCreateRulesComposer()
        {
            _gameMock = new Mock<IGame>();
            _composer = new RulesComposer();
            _composer.SetGame(_gameMock.Object);
        }

        [Test]
        public void CanAddAssistant()
        {
            Assert.DoesNotThrow(() =>
            {
                var assistantMock = new Mock<IRuleAssistant>();
                _composer.Add(assistantMock.Object);
            });
        }

        [Test]
        public void AcceptMove_ShouldCallAllAssistants()
        {
            var assistantMock = new Mock<IRuleAssistant>();
            assistantMock.Setup(x => x.AcceptMove(It.IsAny<Move>()))
                .Verifiable();

            _composer.Add(assistantMock.Object);

            var move = new Move(Mark.Cross, Positions3X3.Center);
            _composer.AcceptMove(move);

            assistantMock.Verify(x => x.AcceptMove(move), Times.Once());
        }
    }
}
