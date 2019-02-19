using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppObjectPool
{
    public class Student : IDisposable
    {
        public string Name { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Name = null;
                //Free any other managed objects here.
            }

            _disposed = true;
        }
    }
}
