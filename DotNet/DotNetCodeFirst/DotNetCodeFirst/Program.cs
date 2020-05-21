using DotNetCodeFirst.Database;
using System;

namespace DotNetCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {

            var context = new MovieContext();
            MovieDbContextSeeder.Seed(context);

            Console.WriteLine("Hello World!");
        }
    }
}
