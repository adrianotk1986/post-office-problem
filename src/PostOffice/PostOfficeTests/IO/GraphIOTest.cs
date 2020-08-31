using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using PostOffice.IO;
using PostOffice.Models;

namespace PostOfficeTests.IO
{
    [TestFixture]
    public class GraphIOTest
    {
        private string PathToFile { get; set; }
        private readonly string graphFilename = "trechos.txt";

        private string[] validLocations = {"BC", "LS", "LV", "RC", "SF", "WS" };
        
        [SetUp]
        public void Setup()
        {
            PathToFile = @".\temp\test\";
            Directory.CreateDirectory(PathToFile);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(PathToFile, recursive: true);
        }

        [Test]
        public void ParseInputTest()
        {
            // Arrange
            var fromToLocations = new List<string>();
            
            for (int i = 0; i < 2; i++)
            {
                fromToLocations.Add(validLocations[i] + " " + validLocations[i+1]);
            }
            
            using (var writer1 = new StreamWriter(PathToFile + graphFilename))
            {
                foreach (var fromTo in fromToLocations)
                {
                    writer1.WriteLine(fromTo + " 1");
                }
            }
            
            Dictionary<string, Node> expectedGraph = new Dictionary<string, Node>();
            expectedGraph.Add(validLocations[0], new Node(validLocations[0], validLocations[1], 1));
            expectedGraph.Add(validLocations[1], new Node(validLocations[1], validLocations[1], 0));
            expectedGraph[validLocations[1]].edges.Add(new Edge(new Node(validLocations[1]), new Node(validLocations[2]), 1));
            expectedGraph.Add(validLocations[2], new Node(validLocations[2], validLocations[2], 0));
            
            var graphIO = new GraphIO();
            
            // Act
            var graph = graphIO.ParseInput(PathToFile + graphFilename);

            // Assert
            Assert.AreEqual(expectedGraph.Keys, graph.Keys);
        }
    }
}