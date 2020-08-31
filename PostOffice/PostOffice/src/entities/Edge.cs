﻿using System.Security.Principal;

namespace ConsoleApp1
{
    public class Edge
    {
        public int travelTime { get; }
        public Node from { get; }
        public Node to { get; }
        
        public Edge(Node from, Node to, int travelTime)
        {
            this.travelTime = travelTime;
            this.from = from;
            this.to = to;
        }
    }
}