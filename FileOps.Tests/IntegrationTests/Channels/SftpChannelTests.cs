using FileOps.Common;
using FileOps.Configuration.Entities;
using FileOps.Processors.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace FileOps.Tests.IntegrationTests.Channels
{

    [TestClass]
    public class SftpChannelTests
    {

        private readonly DirectoryInfo _workingDirectory =
          new DirectoryInfo(Path.Combine("Channels", "TestData", "WorkingDirectory"));


        private readonly DirectoryInfo _testData =
           new DirectoryInfo(Path.Combine("Common", "Data", "TestDirectory"));


        //private readonly DirectoryInfo _testData =
        //   new DirectoryInfo(Path.Combine("Common", "Data", "TestDirectory"));

        private readonly FromSettings _fromSettings = new FromSettings()
        {
                Path = "/home/ec2-user/From",
                Type = ConfigChannelType.Sftp,
                PrivateKey = Path.Combine("Common", "key.private"),
                Host = "127.0.0.1",
                Port = 22,
                UserName = "ec2-user",
            };



        [TestInitialize]
        public void SetUp()
        {
         
            _fromSettings
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _testData.CopyContentTo(_fromSettings);

        }


        [TestCleanup]
        public void TearDown()
        {
            _workingDirectory.DeleteWithContentIfExists();
            _fromSettings.DeleteWithContentIfExists();
        }


        [TestMethod]
        public void CopyEmptyDirectoryFromSftp()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = "/home/ec2-user/Empty",
                Type = ConfigChannelType.Sftp,
                PrivateKey = Path.Combine("Common", "key.private"),
                Host="127.0.0.1",
                Port=22,
                UserName= "ec2-user",
            };

            _workingDirectory.DeleteWithContentIfExists().CreateIfNotExists();

            SftpChannel sftp = new SftpChannel(_workingDirectory, channelSettings);
            //Act
            var result = sftp.Copy();
            //Assert
            Assert.IsTrue(!result.Any());
        }


        [TestMethod]
        public void CopyFromSftp()
        {
            //Arrange
            var channelSettings = new FromSettings()
            {
                Path = "/home/ec2-user/From",
                Type = ConfigChannelType.Sftp,
                PrivateKey = Path.Combine("Common", "key.private"),
                Host = "127.0.0.1",
                Port = 22,
                UserName = "ec2-user",
            };

            _workingDirectory.DeleteWithContentIfExists().CreateIfNotExists();

            SftpChannel sftp = new SftpChannel(_workingDirectory, channelSettings);
            //Act
            var result = sftp.Copy();
            //Assert
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000451-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000452-0_18.xml"));
            Assert.IsTrue(result.Any(d => d.Name == "EE_FEETRA_TPY_000454-0_18.XML"));
            Assert.IsTrue(result.Any(d => d.Name == "GG_TR_529900G3SW56SHYNPR95_01_20180316_0014_01.zip"));

        }


    }
}
