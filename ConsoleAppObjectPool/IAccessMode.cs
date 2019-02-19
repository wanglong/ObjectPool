using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppObjectPool
{
    public interface IAccessMode<T>
    {
        /// <summary>
        /// 租用对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        T Rent();

        /// <summary>
        /// 返回实例
        /// </summary>
        /// <param name="item"></param>
        void Return(T item);
    }

    public enum AccessModel
    {
        /// <summary>
        /// 先进先出 Queue
        /// </summary>
        FIFO = 0,

        /// <summary>
        /// 后进先出 Stack
        /// </summary>
        LIFO = 1,
    }
}
