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
        private readonly string[] invalidLocations = {"AA", "BB", "CC", "DD", "EE", "FF"};
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
            // Directory.Delete(PathToFile, recursive: true);
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
        
        [Test]
        public void DisconnectedPath()
        {
            // Arrange
            var fromToLocations = new List<string>();
            
            for (int i = 0; i < 3; i += 2)
            {
                fromToLocations.Add(validLocations[i] + " " + validLocations[i+1]);
                // BC LS 1
                // LV RC 1
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
    }
}
