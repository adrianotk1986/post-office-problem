using ConsoleApp1;
using ConsoleApp1.io;

namespace PostOffice
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = GraphParser.parseInput(args[0]);
            var pendingJobs = JobParser.parseInput(args[1]);
        }
    }
}