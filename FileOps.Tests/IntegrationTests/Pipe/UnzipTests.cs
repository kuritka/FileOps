using FileOps.Common;
using FileOps.Pipe;
using FileOps.Tests.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FileOps.Tests.IntegrationTests.FileOpsManager
{
    [TestClass]
    public class UnzipTests
    {
        private readonly FileInfo _unzipMultipleFiles = new FileInfo(Path.Combine("IntegrationTests", "Pipe", "Configuration", "unzip.sftp.TwoFilesInside.settings.json"));
        private readonly FileInfo _settingsUnzipOneFile = new FileInfo(Path.Combine("IntegrationTests", "Pipe", "Configuration", "unzip.sftp.OneFileInside.settings.json"));
        private readonly FileInfo _settingsUnzipZeroFile = new FileInfo(Path.Combine("IntegrationTests", "Pipe", "Configuration", "unzip.sftp.ZeroFileInside.settings.json"));
        private readonly FileInfo _settingsUnzipEmptyfolder = new FileInfo(Path.Combine("IntegrationTests", "Pipe", "Configuration", "unzip.sftp.EmptyFolder.settings.json"));
        private readonly FileInfo _sharedSettings = new FileInfo(Path.Combine("IntegrationTests", "Pipe", "Configuration", "shared.settings.json"));
        private readonly FileInfo _settingsUnzipFromMultipleZips = new FileInfo(Path.Combine("IntegrationTests", "Pipe", "Configuration", "unzip.sftp.AllZip.settings.json"));

        private readonly FileInfo _twoFilesInsideOneZip;
        private readonly FileInfo _oneFileInsideOneZip;
        private readonly FileInfo _noFileInsideOneZip;

        private readonly FileInfo _testFile1;
        private readonly FileInfo _testFile2;


        private readonly DirectoryInfo _testData = new DirectoryInfo(Path.Combine("IntegrationTests", "Pipe", "Data"));

        private readonly DirectoryInfo _workingDirectory =
          new DirectoryInfo(Path.Combine("Pipe", "WorkingDirectory"));


        public UnzipTests()
        {
            _twoFilesInsideOneZip = new FileInfo(Path.Combine(_workingDirectory.FullName, "TwoFilesInsideOneZip.zip"));
            _oneFileInsideOneZip = new FileInfo(Path.Combine(_workingDirectory.FullName, "OneFileInsideOneZip.zip"));
            _noFileInsideOneZip = new FileInfo(Path.Combine(_workingDirectory.FullName, "NoFileInsideOneZip.zip"));


            _testFile1 = new FileInfo(Path.Combine(_workingDirectory.FullName, "testFile1.txt"));
            _testFile2 = new FileInfo(Path.Combine(_workingDirectory.FullName, "testFile2.txt"));

        }


        [TestInitialize]
        public void Initialize()
        {

            _workingDirectory
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            FileProvider.Sftp.FromSettings
               .DeleteWithContentIfExists()
               .CreateIfNotExists();

            FileProvider.Sftp.EmptySettings
               .DeleteWithContentIfExists()
               .CreateIfNotExists();

            FileProvider.Sftp.ToSettings
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _testData.CopyContentTo(FileProvider.Sftp.FromSettings);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _workingDirectory.DeleteWithContentIfExists();
            FileProvider.Sftp.FromSettings.DeleteWithContentIfExists();
            FileProvider.Sftp.EmptySettings.DeleteWithContentIfExists();
            FileProvider.Sftp.ToSettings.DeleteWithContentIfExists();
        }


        [TestMethod]
        public void UnzipSucessfullyFromSftp()
        {
            
            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_unzipMultipleFiles)
                .AddConfiguration(_sharedSettings)
                .Build();


            IFileOpsManager fileOpsManager = new FileOps.Pipe.FileOpsManager(steps,"identifier");

            //Act
            fileOpsManager.Execute();

            //Assert
            FileProvider.Sftp.ToSettings.CopyContentTo(_workingDirectory);

            Assert.AreEqual(2, _workingDirectory.GetFiles().Length);
            Assert.IsTrue(File.Exists(_testFile1.FullName));
            Assert.IsTrue(File.Exists(_testFile2.FullName));
        }


        [TestMethod]
        public void UnzipOneItemFromOneFile()
        {
            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settingsUnzipOneFile)
                .AddConfiguration(_sharedSettings)
                .Build();

            IFileOpsManager fileOpsManager = new FileOps.Pipe.FileOpsManager(steps, "identifier");

            //Act
            fileOpsManager.Execute();

            //Assert
            FileProvider.Sftp.ToSettings.CopyContentTo(_workingDirectory);

            Assert.AreEqual(1, _workingDirectory.GetFiles().Length);
            Assert.IsTrue(File.Exists(_testFile1.FullName));
        }


        public void UnzipFromEmptyFolder()
        {
            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settingsUnzipEmptyfolder)
                .AddConfiguration(_sharedSettings)
                .Build();


            IFileOpsManager fileOpsManager = new FileOps.Pipe.FileOpsManager(steps, "identifier");

            //Act
            fileOpsManager.Execute();

            //Assert

            Assert.AreEqual(0, _workingDirectory.GetFiles().Length);

        }



        [TestMethod]
        public void UnzipZeroItemsFromOneFile()
        {
            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settingsUnzipZeroFile)
                .AddConfiguration(_sharedSettings)
                .Build();

            IFileOpsManager fileOpsManager = new FileOps.Pipe.FileOpsManager(steps, "identifier");

            //Act
            fileOpsManager.Execute();

            //Assert
            FileProvider.Sftp.ToSettings.CopyContentTo(_workingDirectory);

            Assert.AreEqual(0, _workingDirectory.GetFiles().Length);
        }



        [TestMethod]
        public void UnzipMixingFromAllFilesWhereZipsDoesntContainSameFileInfo()
        {
            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settingsUnzipFromMultipleZips)
                .AddConfiguration(_sharedSettings)
                .Build();


            FileProvider.Sftp.FromSettings.DeleteOneFile(_twoFilesInsideOneZip);

            IFileOpsManager fileOpsManager = new FileOps.Pipe.FileOpsManager(steps, "identifier");

            //Act
            fileOpsManager.Execute();

            //Assert
            FileProvider.Sftp.ToSettings.CopyContentTo(_workingDirectory);

            Assert.AreEqual(3, _workingDirectory.GetFiles().Length);
            Assert.IsTrue(File.Exists(_testFile1.FullName));
            Assert.IsTrue(File.Exists(Path.Combine(_workingDirectory.FullName, "ClientInput2.csv")));
            Assert.IsTrue(File.Exists(Path.Combine(_workingDirectory.FullName, "ClientInput1.csv")));
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void UnzipMixingFromAllFilesWhereZipsContainsSameFileInfo()
        {

            //Arrange
            var steps = new FileOpsBuilder()
                .AddConfiguration(_settingsUnzipFromMultipleZips)
                .AddConfiguration(_sharedSettings)
                .Build();

            IFileOpsManager fileOpsManager = new FileOps.Pipe.FileOpsManager(steps, "identifier");

            //Act
            fileOpsManager.Execute();
        }

    }
}
