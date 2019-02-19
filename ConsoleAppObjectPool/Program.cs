using System;

namespace ConsoleAppObjectPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestPool();
            Console.Read();
        }

        public static void TestPool()
        {
            Func<Student> func = NewStudent;
            var pool = new Pool<Student>(AccessModel.FIFO, 2, func);
            for (var i = 0; i < 5; i++)
            {
                Student temp = pool.Rent();
                //todo:Some operations
                pool.Return(temp);
            }

            Student temp01 = pool.Rent();
            Student temp001 = pool.Rent();
            pool.Return(temp001);
            Student temp1 = pool.Rent();

            pool.Return(temp1);
            pool.Return(temp01);
            pool.Dispose();
        }

        public static Student NewStudent()
        {
            return new Student();
        }
    }
}
