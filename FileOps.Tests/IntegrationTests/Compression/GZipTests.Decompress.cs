using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FileOps.Tests.Common;
using FileOps.Processors.Compression;

namespace Beamer.IntegrationTests.Compression
{
    public partial class GZipTests
    {

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "compressedFile")]
        public void UnZipNullFile()
        {
            ICompressor compressor = new GZip();
            _decompressedFile = compressor.Decompress(null, new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "Data"))).FirstOrDefault();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(FileNotFoundException))]
        public void UnZipFileWhichDoesntExist()
        {
            ICompressor compressor = new GZip();
            FileInfo fileToDecompress = new FileInfo(Path.Combine(_workingDirectory.FullName, "EBRD_EMIR_Trades_20170745.gz"));
            _decompressedFile = compressor.Decompress(fileToDecompress, new DirectoryInfo("Data")).FirstOrDefault();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(ArgumentException), ExceptionMessage = "Invalid filename. Check whether EBRD_EMIR_Trades_20170712.xml ends with .gz")]
        public void UnZipFileWithWrongExtension()
        {
            ICompressor compressor = new GZip();
            FileInfo fileToDecompress = new FileInfo(_testFilePath);
            _decompressedFile = compressor.Decompress(fileToDecompress, new DirectoryInfo("Data")).FirstOrDefault();
        }

        [TestMethod]
        [ExtendedExpectedException(typeof(DirectoryNotFoundException))]
        public void UnZipFileToNonExistentDirectory()
        {
            ICompressor compressor = new GZip();
            FileInfo compressedFile = new FileInfo(_zippedFilePath);
            DirectoryInfo newDirectoryPath = new DirectoryInfo(@"NonExistentDir");
            _decompressedFile = compressor.Decompress(compressedFile, newDirectoryPath).FirstOrDefault();
        }

        [TestMethod]
        public void UnZipFileWithUndefinedDirectoryPath()
        {
            ICompressor compressor = new GZip("xml");
            FileInfo compressedFile = new FileInfo(_zippedFilePath);

            _decompressedFile = compressor.Decompress(compressedFile).FirstOrDefault();
			Assert.IsNotNull(_decompressedFile);

            Assert.AreEqual(Path.GetExtension(_decompressedFile.FullName), ".xml");
            Assert.IsTrue(File.Exists(_decompressedFile.FullName));
        }

        [TestMethod]
        public void UnZipFileWithDefinedDirectoryPath()
        {
            ICompressor compressor = new GZip("xml");
            FileInfo compressedFile = new FileInfo(_zippedFilePath);
            string expectedFileName = $"{Path.GetFileNameWithoutExtension(compressedFile.Name)}.xml";

            _decompressedFile = compressor.Decompress(compressedFile, _workingDirectory).FirstOrDefault();
			Assert.IsNotNull(_decompressedFile);

			Assert.AreEqual(expectedFileName, _decompressedFile.Name);
            Assert.IsTrue(File.Exists(_decompressedFile.FullName));
        }

		[TestMethod]
		public void UnZipFileWithDefinedDirectoryPathInsensitiveExtension()
		{
			ICompressor compressor = new GZip("xml");
			FileInfo compressedFile = new FileInfo(Path.Combine(_workingDirectory.FullName, "EBRD_EMIR_Trades_20170809Case.Gz"));
			string expectedFileName = $"{Path.GetFileNameWithoutExtension(compressedFile.Name)}.xml";

			_decompressedFile = compressor.Decompress(compressedFile, _workingDirectory).FirstOrDefault();
			Assert.IsNotNull(_decompressedFile);

			Assert.AreEqual(expectedFileName, _decompressedFile.Name);
			Assert.IsTrue(File.Exists(_decompressedFile.FullName));
		}

		[TestMethod]
        public void UnZipSameFileTwice()
        {
            ICompressor compressor = new GZip("xml");
            FileInfo compressedFile = new FileInfo(_zippedFilePath);

            _decompressedFile = compressor.Decompress(compressedFile).FirstOrDefault();
			Assert.IsNotNull(_decompressedFile);
			_decompressedFile = compressor.Decompress(compressedFile, _decompressedFile.Directory).FirstOrDefault();
			Assert.IsNotNull(_decompressedFile);

			Assert.AreEqual(Path.GetExtension(_decompressedFile.FullName), ".xml");
            Assert.IsTrue(File.Exists(_decompressedFile.FullName));
        }
    }
}
