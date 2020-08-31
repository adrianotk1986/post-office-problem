using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PostOffice.Exceptions;
using PostOffice.Models;

namespace PostOffice.IO
{
    public class JobParser
    {
        public static List<Job> parseInput(string filePath)
        {
            var pendingJobs = new List<Job>();
            var allJobs = System.IO.File.ReadAllLines(filePath);
            
            foreach (var jobLine in allJobs)
            {
                var splitJob = jobLine.Split();
                if (splitJob.Length != 2)
                {
                    throw new ValidationException("Invalid job input content: " + jobLine + "\nEach line must " +
                                                  "have the following format: origin destination\nE.g.: SF WS");
                }
                var source = splitJob[0].ToUpper();
                var destination = splitJob[1].ToUpper();
                pendingJobs.Add(new Job(source, destination));
            }

            return pendingJobs;
        }
        
        /// <summary>
        /// Writes the path and the travel time of the jobs to an output file.
        /// </summary>
        /// <param name="jobsResults">A list os jobs. Each job will be a list containing the path and the travel time.</param>
        /// <param name="fullPathFileName">
        /// The full path of the output file. If not informed, the output file will be written to the current directory
        /// where this application is running, named as 'rotas.txt'.
        /// </param>
        public static void writeToFile(List<List<string>> jobsResults, string fullPathFileName)
        {
            var fileLocation = "";
            if (!String.IsNullOrEmpty(fullPathFileName))
            {
                fileLocation = fullPathFileName;
            }
            else
            {
                fileLocation = Environment.CurrentDirectory + "/rotas.txt";
            }
            
            using (StreamWriter file = new StreamWriter(fileLocation))
            {
                foreach (var jobResults in jobsResults)
                {
                    var travelTime = jobResults.Last();
                    foreach (var location in jobResults)
                    {
                        if (location.Equals(travelTime))
                        {
                            file.WriteLine(location);
                        }
                        else
                        {
                            file.Write(location + " ");                        
                        }
                    }
                }
            }
        }
    }
}