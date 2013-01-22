using System;
using System.Diagnostics.Contracts;
using TicTacToe.Games;

namespace TicTacToe.Player
{
    [ContractClass(typeof(PlayerContracts))]
    public interface IPlayer : IObserver<IGame>
    {
        Mark Mark { get; }
        string Name { get; }
        void Join(IGame game);
    }
}