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
            new DirectoryInfo(Path.Combine("Channels", "TestData","WorkingDirectory"));

        private readonly DirectoryInfo _fromFolder =
            new DirectoryInfo(Path.Combine("Channels", "TestData", "IN"));

        private readonly DirectoryInfo _toFolder =
        new DirectoryInfo(Path.Combine("Channels", "TestData", "OUT"));


        private readonly DirectoryInfo _testData =
            new DirectoryInfo(Path.Combine("Common", "Data", "TestDirectory"));


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
        }

        [TestCleanup]
        public void TearDown()
        {
            _workingDirectory.DeleteWithContentIfExists();
            _fromFolder.DeleteWithContentIfExists();
            _toFolder.DeleteWithContentIfExists();
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
            LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings );
            var files = new DirectoryInfo(channelSettings.Path).GetFiles();
            
            //Act
            var result = channel.Copy(files);

            //Assert
            Assert.AreEqual(result.Count(), _testData.GetFiles().Length);
            Assert.IsTrue(result.Any(d=>d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
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
            var files = _workingDirectory.GetFiles();

            //Act
            var result = channel.Copy(files);

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
            var files = _workingDirectory.GetFiles();

            //Act
            var result = channel.Copy(files);

            //Assert
            Assert.AreEqual(result.Count(), _testData.GetFiles().Length);
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

            var _toDataFiles = _toFolder.GetFiles();
            Assert.AreEqual(_testData.GetFiles().Length*2, _toFolder.GetFiles().Length);
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml_0"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml_0"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML_0"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
            Assert.IsTrue(_toDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip_0"));
        }






        // [TestMethod]
        //public void CopyAllInboundFilesTwice()
        //{
        //    //Arrange
        //    var channelSettings = new FromSettings()
        //    {
        //        Path = _fromFolder.FullName,
        //        Type = ConfigChannelType.Local
        //    };
        //    LocalChannel channel = new LocalChannel(_workingDirectory, channelSettings );
        //    var files = new DirectoryInfo(channelSettings.Path).GetFiles();
            
        //    //Act
        //    channel.Copy(files);
        //    var result = channel.Copy(files);

        //    //Assert
        //    Assert.AreEqual(result.Count(), _testData.GetFiles().Length);
        //    Assert.IsTrue(result.Any(d=>d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
        //    Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
        //    Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
        //    Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

        //    var _fromDataFiles = _fromFolder.GetFiles();
        //    Assert.AreEqual(_testData.GetFiles().Length * 2, _fromFolder.GetFiles().Length);
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000451-0_18.xml{Constants.FileExtensions.FileOps}"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000452-0_18.xml{Constants.FileExtensions.FileOps}"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"EE_FEETRA_TPY_000454-0_18.XML{Constants.FileExtensions.FileOps}"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == $"GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip{Constants.FileExtensions.FileOps}"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
        //    Assert.IsTrue(_fromDataFiles.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));
        //}

    }
}
