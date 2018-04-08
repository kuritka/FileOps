using FileOps.Common;
using FileOps.Configuration.Entities;
using FileOps.Processors.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileOps.Tests.IntegrationTests.Channels
{

    [TestClass]
    public class SftpChannelTests
    {

        private readonly DirectoryInfo _workingDirectory =
          new DirectoryInfo(Path.Combine("Channels", "TestData", "WorkingDirectory"));


        private readonly DirectoryInfo _fromFolder =
         new DirectoryInfo(Path.Combine("Channels", "TestData", "IN"));

        //private readonly DirectoryInfo _testData =
        //   new DirectoryInfo(Path.Combine("Common", "Data", "TestDirectory"));


        [TestInitialize]
        public void SetUp()
        {
            _workingDirectory
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _fromFolder
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            //_toFolder
            //    .DeleteWithContentIfExists()
            //    .CreateIfNotExists();

            //_testData.CopyContentTo(_fromFolder);

            //_fromEmpty.CreateIfNotExists();
        }


        [TestCleanup]
        public void TearDown()
        {
            _workingDirectory.DeleteWithContentIfExists();
            _fromFolder.DeleteWithContentIfExists();
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
            SftpChannel sftp = new SftpChannel(_workingDirectory, channelSettings);
            //Act
            var result = sftp.Copy();
            //Assert
            Assert.IsTrue(!result.Any());
        }




    }
}
