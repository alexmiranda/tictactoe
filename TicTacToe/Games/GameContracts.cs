using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Rules;
using TicTacToe.Player;

namespace TicTacToe.Games
{
    [ContractClassFor(typeof(IGame))]
    internal abstract class GameContracts : IGame
    {
        public abstract GameStatus Status { get; }

        public abstract IDisposable Subscribe(IObserver<IGame> observer);

        public void Start(IRuleAssistant assistant)
        {
            Contract.Ensures(Status != GameStatus.New);
        }

        void IGame.Play(Mark mark, IPosition position, params object[] options)
        {
            Contract.Requires<BadMoveException>(mark != Mark.Blank, "Cannot play blank on any position");
            Contract.Requires<ArgumentNullException>(position != null, "position");
        }

        public abstract void Quit();

        public abstract IReadOnlyList<Move> Moves { get; }

        [ContractInvariantMethod]
        private void ContractInvariants()
        {
            Contract.Invariant(Status != null, "Status cannot be null");
            Contract.Invariant(Moves != null, "Moves cannot be null");
        }
    }
}
