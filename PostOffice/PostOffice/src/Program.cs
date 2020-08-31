using System.Collections.Generic;
using ConsoleApp1;
using ConsoleApp1.graph;
using ConsoleApp1.io;

namespace PostOffice
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = GraphParser.parseInput(args[0]);
            var pendingJobs = JobParser.parseInput(args[1]);
            
            var resultFile = "";
            if (args.Length == 3)
            {
                resultFile = args[2];
            }
            
            var jobsResults = new List<List<string>>();
            foreach (var pendingJob in pendingJobs)
            {
                jobsResults.Add(PathFinder.calculateShortestPath(graph, pendingJob));
            }
            
            JobParser.writeToFile(jobsResults, resultFile);
        }
    }
}