using System.Collections.Generic;
using PostOffice.Models;

namespace PostOffice.IO.Interfaces
{
    public interface IGraphInputReader
    {
        public Dictionary<string, Node> ParseInput(string pathsFileLocation);
    }
}