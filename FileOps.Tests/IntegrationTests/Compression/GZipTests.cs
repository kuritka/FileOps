using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileOps.Tests.Common;
using FileOps.Processors.Compression;
using FileOps.Common;

namespace Beamer.IntegrationTests.Compression
{
    [TestClass]
    public partial class GZipTests
    {
        private FileInfo _compressedFile;
        private FileInfo _decompressedFile;

        private readonly string _testFilePath;
        private readonly string _zippedFilePath;
        private readonly string _targetZipFileName;


        private readonly DirectoryInfo _workingDirectory =
            new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "WorkingDirectory2"));


        private readonly DirectoryInfo _testData =
            new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "Data"));



        public GZipTests()
        {
            _testFilePath = Path.Combine(_workingDirectory.FullName,"EBRD_EMIR_Trades_20170712.xml");
            _zippedFilePath = Path.Combine(_workingDirectory.FullName, "EBRD_EMIR_Trades_20170809.gz");
            _targetZipFileName = "EBRD_EMIR_Trades.gz";
        }

        [TestInitialize]
        public void Initialize()
        {

            _workingDirectory
                .DeleteWithContentIfExists()
                .CreateIfNotExists();


            _testData.CopyContentTo(_workingDirectory);


            _compressedFile = null;
            _decompressedFile = null;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _workingDirectory.DeleteWithContentIfExists();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "filesToCompress")]
        public void ZipNullFile()
        {
            ICompressor compressor = new GZip();
            _compressedFile = compressor.Compress(null, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "compressedFileName")]
        public void ZipFileWithUndefinedCompressedFile()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };
            _compressedFile = compressor.Compress(filesToCompress, null).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(DirectoryNotFoundException))]
        public void ZipFileToNonExistentDirectory()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };
            FileInfo zipFile = new FileInfo(Path.Combine("NonExistentDir", _targetZipFileName));

            _compressedFile = compressor.Compress(filesToCompress, zipFile).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentException), ExceptionMessage = "Invalid filesToCompress List filesToCompress contains more than one file. GZip does not support compression of multiple files to one GZip archive.")]
        public void ZipMultipleFiles()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo>
            {
                new FileInfo(_testFilePath),
                new FileInfo(_testFilePath)
            };

            _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentException), ExceptionMessage = "Invalid filesToCompress List filesToCompress is empty")]
        public void ZipEmptyFileList()
        {
            ICompressor compressor = new GZip();
			_compressedFile = compressor.Compress(new List<FileInfo>(), new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(FileNotFoundException))]
        public void ZipFileWhichDoesntExist()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(Path.Combine("Data", "EBRD_EMIR_Trades_20170712.xm1l")) };

            _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentException), ExceptionMessage = "Invalid xyz.txt filename. Check whether filesToCompress ends with .gz")]
        public void ZipToFileWithWrongExtension()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };

            _compressedFile = compressor.Compress(filesToCompress, new FileInfo("xyz.txt")).DefaultIfEmpty(new FileInfo("mock")).Single();
        }

        [TestMethod]
        public void ZipFileWithDefinedName()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };

            _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();

            Assert.AreEqual(_compressedFile.Name, _targetZipFileName);
            Assert.IsTrue(File.Exists(_compressedFile.FullName));
        }

        [TestMethod]
        public void ZipSameFileTwice()
        {
            ICompressor compressor = new GZip();
            List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };

            _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
            _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();

            Assert.AreEqual(Path.GetExtension(_compressedFile.FullName), ".gz");
            Assert.IsTrue(File.Exists(_compressedFile.FullName));
        }
    }
}
