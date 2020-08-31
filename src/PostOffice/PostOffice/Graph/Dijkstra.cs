using System.Collections.Generic;
using System.Linq;
using PostOffice.Graph.Interfaces;
using PostOffice.Models;

namespace PostOffice.Graph
{
    public class Dijkstra : IDijkstra
    {
        private const int NoTravelTime = 0;

        public List<string> CalculateShortestPath(Dictionary<string, Node> graph, Job job)
        {
            var trails = new Dictionary<string, string>();
            var unvisitedNodes = new List<Node>(graph.Values);
            
            // Initialize all travel times as INFINITE
            var travelTimes = graph.Keys.ToDictionary(nodeName => nodeName, nodeName => int.MaxValue);
            // Travel time from source to itself is 0
            travelTimes[job.from] = NoTravelTime;
            
            while (unvisitedNodes.Any())
            {
                var nextNode = getNextVisitingNode(graph, job.from, unvisitedNodes, travelTimes);

                // Once visited, we'll never visit it again.
                unvisitedNodes.Remove(nextNode);
                
                foreach (var edge in nextNode.edges)
                {
                    // We sum up the edge's travel time with the summed travel time from the previous node.
                    var newTravelTime = edge.travelTime + travelTimes[edge.from.name];
                    // If it's lower than the currents node travel time, we update it with the new sum.
                    if (newTravelTime < travelTimes[edge.to.name])
                    {
                        trails[edge.to.name] = edge.from.name;
                        travelTimes[edge.to.name] = newTravelTime;
                    }
                }

                if (nextNode == graph[job.to])
                {
                    return ConstructPath(job.to, trails, travelTimes);
                }
            }
            return ConstructPath(job.to, trails, travelTimes);
        }

        /// <summary>
        /// Get the next visiting node from the unvisited nodes in a greedy way.
        /// The chosen is the one that has the lowest travel time. 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startingNode"></param>
        /// <param name="unvisitedNodes"></param>
        /// <param name="travelTimes"></param>
        /// <returns></returns>
        private Node getNextVisitingNode(Dictionary<string, Node> graph, string startingNode, List<Node> unvisitedNodes,
            Dictionary<string, int> travelTimes)
        {
            var minTravelTime = int.MaxValue;
            var nextNode = graph[startingNode];
            
            foreach (var node in unvisitedNodes)
            {
                if (travelTimes[node.name] <= minTravelTime)
                {
                    minTravelTime = travelTimes[node.name];
                    nextNode = node;
                }
            }

            return nextNode;
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
        private List<string> ConstructPath(string destination, IReadOnlyDictionary<string, string> trails, 
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