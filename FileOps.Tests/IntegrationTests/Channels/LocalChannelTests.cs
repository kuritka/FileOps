using FileOps.Common;
using FileOps.Configuration.Entities;
using FileOps.Processors.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace FileOps.Tests.IntegrationTests.Channels
{

    [TestClass]
    public class LocalChannelTests
    {

        private readonly DirectoryInfo _workingDirectory =
            new DirectoryInfo(Path.Combine("Channels", "TestData", "WorkingDirectory"));

        private readonly DirectoryInfo _fromFolder =
            new DirectoryInfo(Path.Combine("Channels", "TestData", "IN"));

        private readonly DirectoryInfo _toFolder =
        new DirectoryInfo(Path.Combine("Channels", "TestData", "OUT"));


        private readonly DirectoryInfo _testData =
            new DirectoryInfo(Path.Combine("Common", "Data", "TestDirectory"));

        private readonly DirectoryInfo _fromEmpty =
         new DirectoryInfo(Path.Combine("Channels", "TestData", "Empty"));



        [TestInitialize]
        public void SetUp()
        {
            _workingDirectory
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _fromFolder
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _toFolder
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _testData.CopyContentTo(_fromFolder);

            _fromEmpty.CreateIfNotExists();
        }

        [TestCleanup]
        public void TearDown()
        {
            _workingDirectory.DeleteWithContentIfExists();
            _fromFolder.DeleteWithContentIfExists();
            _toFolder.DeleteWithContentIfExists();
            _fromEmpty.DeleteWithContentIfExists();
        }


        [TestMethod]
        public void CopyInboundAllFilesWithoutLocalBackup()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                Type = ConfigChannelType.Local
            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);
            //var files = new DirectoryInfo(channelSettings.Path).GetFiles();

            //Act
            var result = channel.Copy();

            //Assert
            Assert.AreEqual(result.Count(), _testData.GetFiles().Length);
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            var _fromDataFiles = _fromFolder.GetFiles();
            Assert.AreEqual(_testData.GetFiles().Length * 2, _fromFolder.GetFiles().Length);
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000451-0_18.xml{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000452-0_18.xml{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000454-0_18.XML{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }





        [TestMethod]
        public void CopyOutboundAllFilesWithoutLocalBackup()
        {
            //Arrange
            var channelSettings = new ToSettings()
            {
                Path = _toFolder.FullName,
                Type = ConfigChannelType.Local
            };
            _testData.CopyContentTo(_workingDirectory);
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            Assert.AreEqual(result.Count(), _testData.GetFiles().Length);
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            var _toDataFiles = _toFolder.GetFiles();
            Assert.AreEqual(_testData.GetFiles().Length, _toFolder.GetFiles().Length);
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }



        [TestMethod]
        public void CopyOutboundAllFilesWithoutLocalBackupWithSuffix()
        {
            //Arrange
            var channelSettings = new ToSettings()
            {
                Path = _toFolder.FullName,
                Type = ConfigChannelType.Local,
                SuccessFileUploadSuffix = "_0"
            };
            _testData.CopyContentTo(_workingDirectory);
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            Assert.AreEqual(result.Count(), _testData.GetFiles().Length);
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            var _toDataFiles = _toFolder.GetFiles();
            Assert.AreEqual(_testData.GetFiles().Length * 2, _toFolder.GetFiles().Length);
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml_0"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml_0"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML_0"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip_0"));
        }


        [TestMethod]
        public void CopyAllInboundFilesTwice()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                Type = ConfigChannelType.Local
            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result1 = channel.Copy();
            var result2 = channel.Copy();

            //Assert
            foreach (var item in result1)
            {
                Assert.IsTrue(result2.Any(d => d.Name == item.Name));
            }
        }



        [TestMethod]
        public void CopyByFilemaskOnly()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                FileMask = "EE_*_*_*-0_18.xml",
                Type = ConfigChannelType.Local
            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));

            Assert.AreEqual(6, sourceFiles.Count());
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000451-0_18.xml{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000452-0_18.xml{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }


        [TestMethod]
        public void CopyWithExclusionFilemaskOnly()
        {

            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                ExclusionFileMasks = new string[] { "EE_*_*_*-0_18.xml"},
                Type = ConfigChannelType.Local
            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            Assert.AreEqual(6, sourceFiles.Count());
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000454-0_18.XML{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

        }

        [TestMethod]
        public void CopyWithExclusionFilemasksOnly()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                ExclusionFileMasks = new string[] { "EE_*_*_*-0_18.xml", "*.XML" },
                Type = ConfigChannelType.Local
            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            Assert.AreEqual(5, sourceFiles.Count());
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }



        [TestMethod]
        public void CopyWithExclusionFilemaskAndFilemask()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                FileMask = "*.zip",
                ExclusionFileMasks = new string[] { "EE_*_*_*1-0_18.xml", "*.XML" },
                Type = ConfigChannelType.Local
            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            Assert.AreEqual(5, sourceFiles.Count());
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }


        [TestMethod]
        public void CopyWithIgnoreUpperCaseFilemask()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                FileMask = "*.ZIP",
                ExclusionFileMasks = new string[] {"*.XML" },
                Type = ConfigChannelType.Local,
                IgnoreCaseSensitive = true

            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            Assert.AreEqual(5, sourceFiles.Count());
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == $"GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip{Constants.FileExtensions.FileOps}"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }


        [TestMethod]
        public void FilemaskSettedToDoNotFetchAnyFiles()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromFolder.FullName,
                FileMask = "*.txt",
                ExclusionFileMasks = new string[] { "*.XML", "*.zip" },
                Type = ConfigChannelType.Local,
                IgnoreCaseSensitive = true

            };
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(0, result.Count());

            Assert.AreEqual(4, sourceFiles.Count());
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(sourceFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        }


        [TestMethod]
        public void CopyFromEmptyDirectoryFilemask()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = _fromEmpty.FullName,
                FileMask = "*.xml",
                ExclusionFileMasks = new string[] {  },
                Type = ConfigChannelType.Local,
                IgnoreCaseSensitive = true

            };

            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings);

            //Act
            var result = channel.Copy();

            //Assert
            var sourceFiles = new DirectoryInfo(channelSettings.Path).GetFiles();
            Assert.AreEqual(0, result.Count());

            Assert.AreEqual(0, sourceFiles.Count());
        }
    }
}
