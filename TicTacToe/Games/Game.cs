﻿using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;
using TicTacToe.Games.Rules;
using TicTacToe.Support;

namespace TicTacToe.Games
{
    internal class Game : Subject<IGame>, IGame
    {
        private readonly object _pencil = new object();
        
        private IGrid _grid;
        private IRuleAssistant _assistant;

        private readonly List<Move> _moves = new List<Move>(9);  

        public Game(IGrid grid)
        {
            EnsuresGridIsNotNull(grid);
            _grid = grid;
            Status = GameStatus.New;
        }

        public virtual GameStatus Status { get; private set; }

        public virtual void Start(IRuleAssistant assistant = null)
        {
            InitialiseAssistant(assistant);

            lock (_pencil)
            {
                Status = Status.ToStarted();
                CheckWhetherThereAreCompletedRows();
            }
        }

        private void CheckWhetherThereAreCompletedRows()
        {
            if (_grid.CompletedRows().Any())
            {
                Status = Status.ToOver();
                NotifyCompleted();
            } else
            {
                Notify(this);
            }
        }

        private void InitialiseAssistant(IRuleAssistant assistant)
        {
            _assistant = assistant ?? RuleAssistant.Null;
            _assistant.SetGame(this);
        }

        public virtual void Play(Mark mark, IPosition position, params object[] options)
        {
            lock (_pencil)
            {
                try
                {
                    EnsuresGameIsStarted();
                    EnsuresPositionIsValid(position);
                    EnsuresDifferentPlayer(mark);
                    EnsuresPositionsIsNotFilled(position);

                    var move = new Move(mark, position);
                    _assistant.AcceptMove(move);
                    _grid = mark.On(_grid, position);
                    _moves.Add(move);

                    CheckWhetherThereAreCompletedRows();

                } catch (Exception ex)
                {
                    NotifyError(ex);
                    throw;
                }
            }
        }

        public virtual void Quit()
        {
            lock (_pencil)
            {
                EnsuresGameIsStarted();
                Status = Status.ToOver();
                NotifyCompleted();
            }
        }

        public virtual IGrid Grid { get { return _grid; } }

        public virtual IEnumerable<Move> Moves 
        { 
            get { return _moves.AsReadOnly(); }
        }

        private void EnsuresGameIsStarted()
        {
            if (!Status.IsStarted())
                throw new GameNotStartedException();
        }

        private static void EnsuresGridIsNotNull(IGrid grid)
        {
            if (grid == null)
                throw new ArgumentNullException("grid");
        }

        private void EnsuresPositionsIsNotFilled(IPosition position)
        {
            if (_grid.IsFilled(position))
                throw new FilledPositionException();
        }

        private void EnsuresDifferentPlayer(Mark mark)
        {
            var lastMove = Moves.LastOrDefault();
            if (lastMove != null && lastMove.Mark.Equals(mark))
                throw new GameSequenceException();
        }

        private void EnsuresPositionIsValid(IPosition position)
        {
            if (!(position is Positions3X3))
                throw new ArgumentException("position");
        }
    }
}