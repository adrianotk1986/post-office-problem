using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using PostOffice;

namespace PostOfficeTests
{
    [TestFixture]
    public class ProgramTest
    {
        public string PathToFile { get; set; }
        private readonly string[] validLocations = {"BC", "LS", "LV", "RC", "SF", "WS"};
        private readonly string graphFilename = "trechos.txt";
        private readonly string jobsFilename = "encomendas.txt";
        private readonly string expectedRoutesFilename = "expectedRotas.txt";
        private readonly string routesFilename = "rotas.txt";
        
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
        
        private bool EqualFiles(string filePath1, string filePath2)
        {
            byte[] expectedRotasMD5;
            byte[] rotasMD5;
            using (var md5 = MD5.Create())
            {
                using (var stream1 = File.OpenRead(filePath1))
                using (var stream2 = File.OpenRead(filePath2))
                {
                    expectedRotasMD5 = md5.ComputeHash(stream1);
                    rotasMD5 = md5.ComputeHash(stream2);
                }
            }

            return expectedRotasMD5.SequenceEqual(rotasMD5);
        }

        /// <summary>
        /// This test covers the case where the source is adjacent to the destination.
        /// </summary>
        [Test]
        public void OneEdgePath()
        {
            // Arrange
            var fromToLocations = new List<string>();
            
            for (int i = 0; i < validLocations.Length - 1; i++)
            {
                fromToLocations.Add(validLocations[i] + " " + validLocations[i+1]);
            }
            
            using (var writer1 = new StreamWriter(PathToFile + graphFilename))
            using (var writer2 = new StreamWriter(PathToFile + expectedRoutesFilename))
            {
                foreach (var fromTo in fromToLocations)
                {
                    writer1.WriteLine(fromTo + " 1");
                    writer2.WriteLine(fromTo + " 1");
                }
            }
            
            using (var writer = new StreamWriter(PathToFile + jobsFilename))
            {
                foreach (var fromTo in fromToLocations)
                {
                    writer.WriteLine(fromTo);
                }
            }

            string[] args = {PathToFile + graphFilename, PathToFile + jobsFilename, PathToFile + routesFilename};
            
            // Act
            Program.Main(args);
     
            // Assert
            Assert.True(EqualFiles(PathToFile + expectedRoutesFilename, PathToFile + routesFilename));
        }
        
        /// <summary>
        /// This test covers the case where the destination is unreachable.
        /// </summary>
        [Test]
        public void DisconnectedPath()
        {
            // Arrange
            var fromToLocations = new List<string>();
            
            for (int i = 0; i < 3; i += 2)
            {
                fromToLocations.Add(validLocations[i] + " " + validLocations[i+1]);
            }
            
            using (var writer = new StreamWriter(PathToFile + graphFilename))
            {
                foreach (var fromTo in fromToLocations)
                {
                    writer.WriteLine(fromTo + " 1");
                }
            }
            
            using (var writer1 = new StreamWriter(PathToFile + jobsFilename))
            using (var writer2 = new StreamWriter(PathToFile + expectedRoutesFilename))
            {
                writer1.WriteLine(validLocations[0] + " " + validLocations[2]);
                writer2.WriteLine(validLocations[0] + " " + validLocations[2] + " " + int.MaxValue);
            }

            string[] args = {PathToFile + graphFilename, PathToFile + jobsFilename, PathToFile + routesFilename};
            
            // Act
            Program.Main(args);
     
            // Assert
            Assert.True(EqualFiles(PathToFile + expectedRoutesFilename, PathToFile + routesFilename));
        }
        
        /// <summary>
        /// This test covers the given example case.
        /// </summary>
        [Test]
        public void ExamplePath()
        {
            // Arrange
            using (var writer = new StreamWriter(PathToFile + graphFilename))
            {
                writer.WriteLine("LS SF 1");
                writer.WriteLine("SF LS 2");
                writer.WriteLine("LS LV 1");
                writer.WriteLine("LV LS 1");
                writer.WriteLine("SF LV 2");
                writer.WriteLine("LV SF 2");
                writer.WriteLine("LS RC 1");
                writer.WriteLine("RC LS 2");
                writer.WriteLine("SF WS 1");
                writer.WriteLine("WS SF 2");
                writer.WriteLine("LV BC 1");
                writer.WriteLine("BC LV 1");
            }

            using (var writer = new StreamWriter(PathToFile + jobsFilename))
            {
                writer.WriteLine("SF WS");
                writer.WriteLine("LS BC");
                writer.WriteLine("WS BC");
            }
            using (var writer = new StreamWriter(PathToFile + expectedRoutesFilename))
            {
                writer.WriteLine("SF WS 1");
                writer.WriteLine("LS LV BC 2");
                writer.WriteLine("WS SF LV BC 5");
            }

            string[] args = {PathToFile + graphFilename, PathToFile + jobsFilename, PathToFile + routesFilename};
            
            // Act
            Program.Main(args);
     
            // Assert
            Assert.True(EqualFiles(PathToFile + expectedRoutesFilename, PathToFile + routesFilename));
        }
        
        /// <summary>
        /// This test covers the case where the destination is adjacent to the source,
        /// but the travel time is greater than the other adjacent node.
        /// Although the travel time for each edge of the path is small, the summed travel time
        /// is greater.
        /// </summary>
        [Test]
        public void AdjacentNodePath()
        {
            // Arrange
            using (var writer = new StreamWriter(PathToFile + graphFilename))
            {
                writer.WriteLine("BC LS 1");
                writer.WriteLine("LS LV 1");
                writer.WriteLine("LV RC 1");
                writer.WriteLine("RC SF 1");
                writer.WriteLine("SF WS 1");
                writer.WriteLine("BC WS 4");
            }

            using (var writer = new StreamWriter(PathToFile + jobsFilename))
            {
                writer.WriteLine("BC WS");
            }
            using (var writer = new StreamWriter(PathToFile + expectedRoutesFilename))
            {
                writer.WriteLine("BC WS 4");
            }

            string[] args = {PathToFile + graphFilename, PathToFile + jobsFilename, PathToFile + routesFilename};
            
            // Act
            Program.Main(args);
     
            // Assert
            Assert.True(EqualFiles(PathToFile + expectedRoutesFilename, PathToFile + routesFilename));
        }
        
        /// <summary>
        /// This test covers the given example case, but with duplicate edges.
        /// </summary>
        [Test]
        public void ExampleWithDuplicateEdgesPath()
        {
            // Arrange
            using (var writer = new StreamWriter(PathToFile + graphFilename))
            {
                writer.WriteLine("LS SF 1");
                writer.WriteLine("LS SF 1");
                writer.WriteLine("LS SF 1");
                writer.WriteLine("LS SF 1");
                writer.WriteLine("SF LS 2");
                writer.WriteLine("LS LV 1");
                writer.WriteLine("LV LS 1");
                writer.WriteLine("SF LV 2");
                writer.WriteLine("LV SF 2");
                writer.WriteLine("LS RC 1");
                writer.WriteLine("RC LS 2");
                writer.WriteLine("SF WS 1");
                writer.WriteLine("WS SF 2");
                writer.WriteLine("WS SF 2");
                writer.WriteLine("WS SF 2");
                writer.WriteLine("LV BC 1");
                writer.WriteLine("BC LV 1");
            }

            using (var writer = new StreamWriter(PathToFile + jobsFilename))
            {
                writer.WriteLine("SF WS");
                writer.WriteLine("LS BC");
                writer.WriteLine("WS BC");
            }
            using (var writer = new StreamWriter(PathToFile + expectedRoutesFilename))
            {
                writer.WriteLine("SF WS 1");
                writer.WriteLine("LS LV BC 2");
                writer.WriteLine("WS SF LV BC 5");
            }

            string[] args = {PathToFile + graphFilename, PathToFile + jobsFilename, PathToFile + routesFilename};
            
            // Act
            Program.Main(args);
     
            // Assert
            Assert.True(EqualFiles(PathToFile + expectedRoutesFilename, PathToFile + routesFilename));
        }
    }
}
