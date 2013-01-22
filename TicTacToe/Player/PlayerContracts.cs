using System;
using System.Diagnostics.Contracts;
using TicTacToe.Games;

namespace TicTacToe.Player
{
    [ContractClassFor(typeof(IPlayer))]
    internal abstract class PlayerContracts : IPlayer
    {
        public abstract Mark Mark { get; }

        public abstract string Name { get; }

        public void Join(IGame game)
        {
            Contract.Requires<ArgumentNullException>(game != null, "game");
        }

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(IGame value);

        [ContractInvariantMethod]
        private void ContractInvariants()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Name), "Class invariant: Name cannot be null");
            Contract.Invariant(Mark != Mark.Blank, "Class invariant: Mark cannot be blank");
        }
    }
}
