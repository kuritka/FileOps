using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beamer.Infrastructure.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileOps.Common;

namespace Beamer.IntegrationTests.Compression
{
    [TestClass]
    public class PasswordProtectedZipTests
    {

        private readonly List<FileInfo> _testFilePath;
        private readonly FileInfo _outputfile;
        private readonly DirectoryInfo _workingDirectory;

        public PasswordProtectedZipTests()
        {
            _workingDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "PasswordProtectedZip"));
            _testFilePath = new List<FileInfo>
            {
                new FileInfo(Path.Combine("IntegrationTests","Compression","Data", "testFile1.txt")),
                new FileInfo(Path.Combine("IntegrationTests","Compression","Data", "testFile2.txt"))
            };
            _outputfile = new FileInfo(Path.Combine(_workingDirectory.FullName, "test.zip"));            
        }

        [TestMethod]
        public void SucessfullyZipped()
        {
            //arrange
            var zip = new PasswordProtectedZip("hello");
            
            //act
            var compressed = zip.Compress(_testFilePath, _outputfile);
            //assert

            File.Exists(compressed.Single().FullName);
        }


        [TestMethod]
        public void EmptyFilesList()
        {
            //arrange
            var zip = new PasswordProtectedZip("hello");

            //act
            var compressed = zip.Compress(new List<FileInfo>(), _outputfile);

            //assert
            Assert.IsTrue(compressed.IsEmpty);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullOutputFile()
        {
            //arrange
            var zip = new PasswordProtectedZip("hello");

            //act
            var compressed = zip.Compress(_testFilePath, null);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyPassword()
        {
            //arrange
            //act
            //assert
            var zip = new PasswordProtectedZip(string.Empty);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullPassword()
        {
            //arrange
            //act
            //assert
            var zip = new PasswordProtectedZip(null);
        }



        [TestInitialize]
        public void SetUp()
        {
            _workingDirectory
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

        }

        [TestCleanup]
        public void TearDown()
        {
            _workingDirectory
                .DeleteWithContentIfExists();
        }

    }
}
