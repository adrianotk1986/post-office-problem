﻿using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.graph
{
    public class PathFinder
    {
        private const int NoTravelTime = 0;

        public static List<string> calculateShortestPath(Dictionary<string, Node> graph, Job job)
        {
            var unvisitedNodes = new List<Node>();
            var trails = new Dictionary<string, string>();
            var travelTimes = new Dictionary<string, int>();
            
            foreach (var (nodeName, node) in graph)
            {
                if (nodeName.Equals(job.from))
                {
                    // Travel time from source to itself is 0
                    travelTimes.Add(node.name, NoTravelTime);
                }
                else
                {
                    // Initialize all other travel times as INFINITE
                    travelTimes.Add(node.name, int.MaxValue);
                }
                unvisitedNodes.Add(node);
            }

            while (unvisitedNodes.Any())
            {
                var nextNode = graph[job.from];
                var minTravelTime = int.MaxValue;
                
                // Find the best next candidate to be visited
                foreach (var node in unvisitedNodes)
                {
                    if (travelTimes[node.name] <= minTravelTime)
                    {
                        minTravelTime = travelTimes[node.name];
                        nextNode = node;
                    }
                }
                
                // Once visited, we'll never visit it again.
                unvisitedNodes.Remove(nextNode);
                
                foreach (var edge in nextNode.connections)
                {
                    var newTravelTime = edge.travelTime + travelTimes[edge.from.name];
                    if (newTravelTime < travelTimes[edge.to.name])
                    {
                        trails[edge.to.name] = edge.from.name;
                        travelTimes[edge.to.name] = newTravelTime;
                    }
                }
            }
            return constructPath(job.to, trails, travelTimes);
        }
        
        /// <summary>
        /// Constructs the path starting from the destination until the source node, and also adds the total
        /// travel time for the path.
        /// </summary>
        /// <param name="destination">The name of the destination node.</param>
        /// <param name="trails">The path trails that will be used to construct the path.</param>
        /// <param name="travelTimes">The travel times from the source node to the other nodes.</param>
        /// <returns>A list of strings, containing the ordered locations and the last element being the total travel
        ///     time for the path.
        /// </returns>
        private static List<string> constructPath(string destination, IReadOnlyDictionary<string, string> trails, 
            IReadOnlyDictionary<string, int> travelTimes)
        {
            // The destination and its previous node is the starting point.
            var path = new List<string> {destination};
            var previousNode = trails[destination];
            path.Add(previousNode);
            
            // We go through the trails until we reach the starting node.
            while (trails.TryGetValue(previousNode, out var previousNodeValue))
            {
                path.Add(previousNodeValue);
                previousNode = previousNodeValue;
            }

            path.Reverse();
            
            path.Add(travelTimes[destination].ToString());

            return path;
        }
    }
}