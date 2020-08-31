using System.Collections.Generic;
using PostOffice.Models;

namespace PostOffice.IO.Interfaces
{
    public interface IJobInputReader
    {
        public List<Job> ParseInput(string filePath);
    }
}