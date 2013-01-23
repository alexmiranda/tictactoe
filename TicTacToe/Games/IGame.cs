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
        GameStatus Status { get; }
        void Start(IRuleAssistant assistant = null);
        void Play(Mark mark, IPosition position, params object[] options);
        void Quit();
        IReadOnlyList<Move> Moves { get; }
    }
}