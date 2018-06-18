using FileOps.Pipe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FileOps.Tests
{
    [TestClass]
    public class FileOpsManagerTests    
    {
        private readonly FileInfo _settings1 = new FileInfo(Path.Combine("IntegrationTests", "FileOpsManager", "Data", "settings1.json"));
        private readonly FileInfo _settings2 = new FileInfo(Path.Combine("IntegrationTests","FileOpsManager", "Data", "settings2.json"));
        private readonly FileInfo _sharedSettings = new FileInfo(Path.Combine("IntegrationTests","FileOpsManager", "Data", "shared.settings.json"));


        [TestMethod]
        [Ignore]
        public void BuildFileOpsManager()
        {

            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settings1)
                .AddConfiguration(_sharedSettings)
                .Build();

            IFileOpsManager manager = new FileOpsManager(steps, "identifier");
            
            //Act
            manager.Execute();

            //Assert
        }



        [TestMethod]
        [Ignore]
        public void BuildMultipleFileOpsManagers()
        {
            var steps1 = new FileOpsBuilder()
                .AddConfiguration(_settings1)
                .AddConfiguration(_sharedSettings)
                .Build();

            IFileOpsManager manager1 = new FileOpsManager(steps1, "identifier");

            var steps2 = new FileOpsBuilder()
              .AddConfiguration(_settings1)
              .AddConfiguration(_sharedSettings)
              .Build();

            IFileOpsManager manager2 = new FileOpsManager(steps1, "identifier");

            manager1.Execute();

            manager2.Execute();
        }


    }
}
