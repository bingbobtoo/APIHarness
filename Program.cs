using Newtonsoft.Json;
using System;
using System.IO;

namespace NewtonSoft
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// get a list of net projects - sln extensions
        /// get a list of csproj and vsproj files - include their repos and solution paths
        /// identify tests and their corresponding projects
        /// have a wait after the 1st async call
        static void Main(string[] args)
        {
            Console.WriteLine("Start Program");
            Sample sample = new Sample();
            sample.DoSomething();
            Console.WriteLine("End Program");
            Console.ReadKey();
            // https://www.pluralsight.com/guides/understand-control-flow-async-await
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file
        }
    }
}
