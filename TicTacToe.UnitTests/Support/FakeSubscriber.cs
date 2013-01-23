using System;
using TicTacToe.Games;

namespace TicTacToe.UnitTests.Support
{
    internal class FakeSubscriber : IObserver<IGame>
    {
        public Action<IGame> OnNextAction { get; set; }
        public Action<Exception> OnErrorAction { get; set; }
        public Action OnCompletedAction { get; set; }

        #region IObserver<IGame> Members

        public void OnNext(IGame value)
        {
            if (OnNextAction != null)
                OnNextAction(value);
        }

        public void OnError(Exception error)
        {
            if (OnErrorAction != null)
                OnErrorAction(error);
        }

        public void OnCompleted()
        {
            if (OnCompletedAction != null)
                OnCompletedAction();
        }

        #endregion
    }
}