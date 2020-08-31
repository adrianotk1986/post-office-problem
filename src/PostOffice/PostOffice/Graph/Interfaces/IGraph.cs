using System.Collections.Generic;
using PostOffice.Models;

namespace PostOffice.Graph.Interfaces
{
    public interface IGraph
    {
        /// <summary>
        /// Calculate the shortest path between two locations in a given graph.
        /// </summary>
        /// <param name="graph">The graph that will be used to find the shortest path.</param>
        /// <param name="job">Job containing the source and destination location.</param>
        /// <returns>A list of ordered locations, from the source location to the destination. The last element of the
        /// list is the total travel time for the shortest path solution.
        /// </returns>
        public List<string> CalculateShortestPath(Dictionary<string, Node> graph, Job job);
    }
}