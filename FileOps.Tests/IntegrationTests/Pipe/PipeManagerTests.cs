using FileOps.Common;
using FileOps.Pipe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FileOps.Tests
{
    [TestClass]
    public class PipeManagerTests    
    {
        private readonly FileInfo _settings1 = new FileInfo(Path.Combine("Data", "settings1.json"));
        private readonly FileInfo _settings2 = new FileInfo(Path.Combine("Data", "settings2.json"));
        private readonly FileInfo _sharedSettings = new FileInfo(Path.Combine("Data", "shared.settings.json"));


        [TestMethod]
        public void BuildPipeManager()
        {
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settings1)
                .AddConfiguration(_sharedSettings)
                .Build();

            IFileOpsManager manager = new FileOpsManager(steps);

            manager.Execute();
        }



    }
}
