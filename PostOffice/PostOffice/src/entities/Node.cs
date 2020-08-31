﻿using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Node
    {
        public string name { get; }
        public List<Edge> connections { get; } = new List<Edge>();
        
        public Node(string sourceName, string destinationName, int travelTime)
        {
            this.name = sourceName;
            this.connections.Add(new Edge(new Node(sourceName), new Node(destinationName), travelTime));
        }

        public Node(string name)
        {
            this.name = name;
        }
    }
}