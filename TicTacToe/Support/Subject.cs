using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicTacToe.Support
{
    public abstract class Subject<TSubject> : IObservable<TSubject>
    {
        private readonly ISet<IObserver<TSubject>> _observers;

        protected Subject()
        {
            _observers = new HashSet<IObserver<TSubject>>();
        }

        public IDisposable Subscribe(IObserver<TSubject> observer)
        {
            _observers.Add(observer);
            return new UnSubscriber<TSubject>(observer, _observers);
        }

        protected async void Notify(TSubject subject)
        {
            await Task.Run(() => Parallel.ForEach(_observers, (observer) => observer.OnNext(subject)));
        }

        protected async void NotifyError(Exception error)
        {
            await Task.Run(() => Parallel.ForEach(_observers, (observer) => observer.OnError(error)));
        }

        protected async void NotifyCompleted()
        {
            await Task.Run(() => Parallel.ForEach(_observers, (observer) => observer.OnCompleted()));
        }

        #region Nested type: UnSubscriber

        private sealed class UnSubscriber<T> : IDisposable
        {
            private bool _disposed;

            private readonly IObserver<T> _observer;
            private readonly ISet<IObserver<T>> _observers;

            internal UnSubscriber(IObserver<T> observer, ISet<IObserver<T>> observers)
            {
                _observer = observer;
                _observers = observers;
            }

            ~UnSubscriber()
            {
                Dispose(false);
            }

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    if (_observer != null && _observers.Contains(_observer))
                        _observers.Remove(_observer);
                }
                _disposed = true;
            }

            #endregion
        }

        #endregion
    }
}