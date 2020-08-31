﻿ namespace PostOffice.Models
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