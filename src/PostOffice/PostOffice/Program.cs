using System;
using System.Collections.Generic;
using System.Linq;
using PostOffice.Exceptions;
using PostOffice.Graph;
using PostOffice.IO;
using PostOffice.Models;

namespace PostOffice
{
    class Program
    {
        static void Main(string[] args)
        {
            // We could implement a strategy/factory design pattern for both the graph and the job IO, which would
            // give the possibility to retrieve their respective data from elsewhere (for instance, from a web service).
            var graphIO = new GraphIO();
            var graph = graphIO.ParseInput(args[0]);
            
            var jobIO = new JobIO();
            var pendingJobs = jobIO.ParseInput(args[1]);
            
            var resultFile = "";
            if (args.Length == 3)
            {
                resultFile = args[2];
            }
            
            var jobsResults = new List<List<string>>();
            var dijkstra = new Dijkstra();
            foreach (var pendingJob in pendingJobs)
            {
                validateJob(graph,pendingJob);
                jobsResults.Add(dijkstra.CalculateShortestPath(graph, pendingJob));
            }
            
            jobIO.WriteToFile(jobsResults, resultFile);
        }

        private static void validateJob(Dictionary<string, Node> graph, Job pendingJob)
        {
            validateLocation(graph, pendingJob.from);
            validateLocation(graph, pendingJob.to);
        }

        private static void validateLocation(Dictionary<string, Node> graph, string location)
        {
            if (!graph.ContainsKey(location))
            {
                throw new UnexpectedLocationException("The given location " + location + " is not "
                                                      + "contained in the following graph " 
                                                      + string.Join(", ", graph.Select(x => x.Key)));
            }
        }
        
    }
}