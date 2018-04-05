using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileOps.Tests.Common;
using FileOps.Processors.Compression;

namespace Beamer.IntegrationTests.Compression
{
	public partial class ZipTests
	{
	    [TestMethod]
	    [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "compressedFile")]
	    public void UnZipNullFile()
	    {
	        ICompressor compressor = new Zip();
	        _decompressedFiles = compressor.Decompress(null, new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "Data")));
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(FileNotFoundException))]
	    public void UnZipFileWhichDoesntExist()
	    {
	        ICompressor compressor = new Zip();
	        FileInfo fileToDecompress = new FileInfo(Path.Combine("Data", "EBRD_EMIR_Trades_20170745.zip"));
	        _decompressedFiles = compressor.Decompress(fileToDecompress, new DirectoryInfo("Data"));
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(ArgumentException), ExceptionMessage = "Invalid filename. Check whether EBRD_EMIR_Trades_20170712.xml ends with .zip")]
	    public void UnZipFileWithWrongExtension()
	    {
	        ICompressor compressor = new Zip();
	        FileInfo fileToDecompress = new FileInfo(_testFilePath);
	        _decompressedFiles = compressor.Decompress(fileToDecompress, new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "Data")));
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(DirectoryNotFoundException))]
	    public void UnZipFileToNonExistentDirectory()
	    {
	        ICompressor compressor = new Zip();
	        FileInfo compressedFile = new FileInfo(_zippedFilePath);
	        DirectoryInfo newDirectoryPath = new DirectoryInfo(@"NonExistentDir");
	        _decompressedFiles = compressor.Decompress(compressedFile, newDirectoryPath);
	    }

	    [TestMethod]
	    public void UnZipFileWithUndefinedDirectoryPath()
	    {
	        ICompressor compressor = new Zip();
	        FileInfo compressedFile = new FileInfo(_zippedFilePath);

	        _decompressedFiles = compressor.Decompress(compressedFile);

	        Assert.IsTrue(_decompressedFiles.All(x => File.Exists(x.FullName)));
	    }

	    [TestMethod]
	    public void UnZipFileWithDefinedDirectoryPath()
	    {
	        ICompressor compressor = new Zip();
	        FileInfo compressedFile = new FileInfo(_zippedFilePath);

	        _decompressedFiles = compressor.Decompress(compressedFile, new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "Data")));

	        Assert.IsTrue(_decompressedFiles.All(x => File.Exists(x.FullName)));
	    }

		[TestMethod]
		public void UnZipFileWithDefinedDirectoryPathInsensitiveExtension()
		{
			ICompressor compressor = new Zip();
			FileInfo compressedFile = new FileInfo(Path.Combine(_workingDirectory.FullName, "EBRD_EMIR_Trades_20170809Case.Zip"));

			_decompressedFiles = compressor.Decompress(compressedFile, _workingDirectory);

			Assert.IsTrue(_decompressedFiles.All(x => File.Exists(x.FullName)));
		}

		[TestMethod]
	    public void UnZipSameFileTwice()
	    {
	        ICompressor compressor = new Zip();
	        FileInfo compressedFile = new FileInfo(_zippedFilePath);

	        _decompressedFiles = compressor.Decompress(compressedFile, _workingDirectory);
	        _decompressedFiles = compressor.Decompress(compressedFile, _workingDirectory);

	        Assert.IsTrue(_decompressedFiles.All(x => File.Exists(x.FullName)));
	    }
    }
}
