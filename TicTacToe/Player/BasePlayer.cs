using System;
using TicTacToe.Games;

namespace TicTacToe.Player
{
    internal abstract class BasePlayer : IPlayer, IDisposable
    {
        private readonly string _name;
        private readonly Mark _mark;

        protected IGame Game;
        private IDisposable _ticket;

        private bool _disposed = false;

        protected BasePlayer(string name, Mark mark)
        {
            EnsuresNameIsValid(name);
            EnsuresMarkIsValid(mark);
            _name = name;
            _mark = mark;
        }

        public Mark Mark
        {
            get { return _mark; } 
        }

        public string Name 
        {
            get { return _name; }
        }

        public virtual void Join(IGame game)
        {
            _ticket = game.SubscribeSafe(this);
            Game = game;
        }

        private void LeaveGame()
        {
            if (_ticket != null)
                _ticket.Dispose();
        }

        public abstract void OnNext(IGame value);
        public abstract void OnError(Exception error);
        public abstract void OnCompleted();

        private static void EnsuresMarkIsValid(Mark mark)
        {
            if (mark == Mark.Blank)
                throw new ArgumentException("mark");
        }

        private static void EnsuresNameIsValid(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
                LeaveGame();
            _disposed = true;
        }

        ~BasePlayer()
        {
            Dispose(false);
        }
    }
}