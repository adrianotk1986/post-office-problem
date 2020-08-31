using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PostOffice.Exceptions;
using PostOffice.IO.Interfaces;
using PostOffice.Models;

namespace PostOffice.IO
{
    public class JobIO : IJobInputReader, IJobOutputWriter
    {
        public List<Job> ParseInput(string filePath)
        {
            var pendingJobs = new List<Job>();
            var allJobs = File.ReadAllLines(filePath);
            
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
        
        public void WriteToFile(List<List<string>> jobsResults, string fullPathFileName)
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