using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Rules;

namespace TicTacToe.Games
{
    [ContractClass(typeof(GameContracts))]
    public interface IGame : IObservable<IGame>
    {
        void Start(IRuleAssistant assistant = null);
        void Play(Mark mark, IPosition position, params object[] options);
        void Quit();
        GameStatus Status { get; }
        IGrid Grid { get; }
        IEnumerable<Move> Moves { get; }
    }
}