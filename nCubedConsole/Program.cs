using System;
using nCubed.EFCore.Repositories;

namespace nCubedConsole
{
    public class Customer { }

    public class Repository : nCubed.EFCore.Repositories.IRepository<Customer>
    {
        public IUnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Repository r = new Repository();
        }
    }
}
