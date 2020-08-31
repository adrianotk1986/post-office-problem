﻿using System.Collections.Generic;

namespace ConsoleApp1
{
    public class GraphParser
    {
        public static Dictionary<string, Node> parseInput(string pathsFileLocation)
        {
            var paths = System.IO.File.ReadAllLines(pathsFileLocation);

            var graph = new Dictionary<string, Node>();
            
            foreach (var path in paths)
            {
                var splitedPath = path.Split();
                if (splitedPath.Length != 3 || !int.TryParse(splitedPath[2], out var travelTime))
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
                    graph[source].connections.Add(new Edge(new Node(source), new Node(destination), travelTime));
                }
            }

            return graph;
        }
    }
}