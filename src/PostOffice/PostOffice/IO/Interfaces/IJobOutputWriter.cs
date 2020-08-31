using System.Collections.Generic;

namespace PostOffice.IO.Interfaces
{
    public interface IJobOutputWriter
    {
        /// <summary>
        /// Writes the path and the travel time of the jobs to an output file.
        /// </summary>
        /// <param name="jobsResults">A list os jobs. Each job will be a list containing the path and the travel time.</param>
        /// <param name="fullPathFileName">
        /// The full path of the output file. If not informed, the output file will be written to the current directory
        /// where this application is running, named as 'rotas.txt'.
        /// </param>
        public void WriteToFile(List<List<string>> jobsResults, string fullPathFileName);
    }
}