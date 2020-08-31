﻿using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Job
    {
        public string from { get; }
        public string to { get; }
        
        public Job(string @from, string to)
        {
            this.@from = @from;
            this.to = to;
        }
        
    }
}