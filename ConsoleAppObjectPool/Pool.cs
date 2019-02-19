using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleAppObjectPool
{
    /// <summary>
    /// 在Pool中如何控制程序池并发，这里我们引入了 Semaphore 以控制并发，这里将严格控制程序池大小，避免内存溢出。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> : IDisposable where T : IDisposable
    {
        private int _capacity;
        private IAccessMode<T> _accessMode;
        private readonly object _locker = new object();
        private readonly Semaphore _semaphore;

        public Pool(AccessModel accessModel, int capacity, Func<T> func)
        {
            _capacity = capacity;
            _semaphore = new Semaphore(capacity, capacity);
            InitialAccessMode(accessModel, capacity, func);
        }

        private void InitialAccessMode(AccessModel accessModel, int capacity, Func<T> func)
        {
            switch (accessModel)
            {
                case AccessModel.FIFO:
                    _accessMode = new FIFOAccessMode<T>(capacity, func);
                    break;
                case AccessModel.LIFO:
                    _accessMode = new LIFOAccessMode<T>(capacity, func);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public T Rent()
        {
            _semaphore.WaitOne();
            return _accessMode.Rent();
        }

        public void Return(T item)
        {
            _accessMode.Return(item);
            _semaphore.Release();
        }

        public void Dispose()
        {
            if (!typeof(IDisposable).IsAssignableFrom(typeof(T))) return;

            lock (_locker)
            {
                while (_capacity > 0)
                {
                    var disposable = (IDisposable)_accessMode.Rent();
                    _capacity--;
                    disposable.Dispose();
                }

                _semaphore.Dispose();
            }
        }
    }
}
