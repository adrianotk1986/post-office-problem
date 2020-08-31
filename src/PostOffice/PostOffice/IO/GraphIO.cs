using System.Collections.Generic;
using System.IO;
using PostOffice.Exceptions;
using PostOffice.IO.Interfaces;
using PostOffice.Models;

namespace PostOffice.IO
{
    public class GraphIO : IGraphInputReader
    {
        public Dictionary<string, Node> ParseInput(string pathsFileLocation)
        {
            var paths = File.ReadAllLines(pathsFileLocation);

            var graph = new Dictionary<string, Node>();
            
            foreach (var path in paths)
            {
                var splitedPath = path.Split();
                if (splitedPath.Length != 3 || !int.TryParse(splitedPath[2], out var travelTime) 
                                            || int.Parse(splitedPath[2]) < 0)
                {
                    throw new ValidationException("Invalid graph input content: " + path + "\nEach line must " +
                                                  "have the following format: origin destination cost \nE.g.: LS SF 1");
                }
                
                var source = splitedPath[0].ToUpper();
                var destination = splitedPath[1].ToUpper();
                
                if (!graph.ContainsKey(source))
                {
                    graph.Add(source, new Node(source, destination, travelTime));
                }
                else
                {
                    graph[source].edges.Add(new Edge(new Node(source), new Node(destination), travelTime));
                }

                if (!graph.ContainsKey(destination))
                {
                    graph.Add(destination, new Node(destination, destination, 0));
                }
                
            }

            return graph;
        }
    }
}