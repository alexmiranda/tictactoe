using System;
using TicTacToe.Games;

namespace TicTacToe.Player
{
    internal class ComputerPlayer : IPlayer, IDisposable
    {
        private readonly string _name;
        private readonly Mark _mark;

        private IGame _game;
        private IDisposable _ticket;

        public ComputerPlayer(string name, Mark mark)
        {
            EnsuresNameIsValid(name);
            EnsuresMarkIsValid(mark);
            _name = name;
            _mark = mark;
        }

        public void Join(IGame game)
        {
            _ticket = game.SubscribeSafe(this);
            _game = game;
        }

        private void LeaveGame()
        {
            if (_ticket != null)
                _ticket.Dispose();
        }

        public void OnNext(IGame value)
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public Mark Mark
        {
            get { return _mark; } 
        }

        public string Name 
        {
            get { return _name; }
        }

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

        ~ComputerPlayer()
        {
            Dispose(false);
        }

        private bool _disposed = false;
    }
}
