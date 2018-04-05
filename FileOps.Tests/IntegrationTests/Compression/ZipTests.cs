using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileOps.Tests.Common;
using FileOps.Processors.Compression;
using FileOps.Common;

namespace Beamer.IntegrationTests.Compression
{
	[TestClass]
	public partial class ZipTests
	{
	    private FileInfo _compressedFile;
	    private IEnumerable<FileInfo> _decompressedFiles;

	    private readonly string _testFilePath;
	    private readonly string _zippedFilePath;
	    private readonly string _targetZipFileName;


        private readonly DirectoryInfo _workingDirectory =
            new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "WorkingDirectory"));


        private readonly DirectoryInfo _testData =
            new DirectoryInfo(Path.Combine("IntegrationTests", "Compression", "Data"));


        public ZipTests()
	    {
	        _testFilePath = Path.Combine(_workingDirectory.FullName, "EBRD_EMIR_Trades_20170712.xml");
	        _zippedFilePath = Path.Combine(_workingDirectory.FullName, "EBRD_EMIR_Trades_20170809.zip");
	        _targetZipFileName = "EBRD_EMIR_Trades.xml2.zip";
        }

        [TestInitialize]
        public void Initialize()
        {
            _workingDirectory
                .DeleteWithContentIfExists()
                .CreateIfNotExists();

            _testData.CopyContentTo(_workingDirectory);
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
	        ICompressor compressor = new Zip();
	        _compressedFile = compressor.Compress(null, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "compressedFileName")]
	    public void ZipFileWithUndefinedCompressedFile()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };

	        _compressedFile = compressor.Compress(filesToCompress, null).DefaultIfEmpty(new FileInfo("mock")).Single();
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(DirectoryNotFoundException))]
	    public void ZipFileToNonExistentDirectory()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };
	        FileInfo zipFile = new FileInfo(Path.Combine("NonExistentDir", _targetZipFileName));

	        _compressedFile = compressor.Compress(filesToCompress, zipFile).DefaultIfEmpty(new FileInfo("mock")).Single();
	    }

	    [TestMethod]
	    public void ZipEmptyFileList()
	    {
	        ICompressor compressor = new Zip();

            OneOrZeroElementCollection<FileInfo> result = compressor.Compress(new List<FileInfo>(), new FileInfo(_targetZipFileName));

			Assert.IsFalse(result.Any());
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(FileNotFoundException))]
	    public void ZipFileWhichDoesntExist()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo>
	        {
	            new FileInfo(_testFilePath),
                new FileInfo(Path.Combine("Data", "EBRD_EMIR_Trades_20170712.xm1l"))
	        };

	        _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
	    }

	    [TestMethod]
	    [ExtendedExpectedException(typeof(ArgumentException), ExceptionMessage = "Invalid xyz.txt filename. Check whether filesToCompress ends with .zip")]
	    public void ZipToFileWithWrongExtension()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };

	        _compressedFile = compressor.Compress(filesToCompress, new FileInfo("xyz.txt")).DefaultIfEmpty(new FileInfo("mock")).Single();
	    }

	    [TestMethod]
	    public void ZipFileWithDefinedName()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo( _testFilePath) };

	        _compressedFile = compressor.Compress(filesToCompress, new FileInfo(Path.Combine(_workingDirectory.FullName, _targetZipFileName))).DefaultIfEmpty(new FileInfo("mock")).Single();

	        Assert.AreEqual(_compressedFile.Name, _targetZipFileName);
	        Assert.IsTrue(File.Exists(_compressedFile.FullName));
        }

	    [TestMethod]
        [ExtendedExpectedException(typeof(IOException))]
	    public void ZipSameFileTwice()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo> { new FileInfo(_testFilePath) };

	        _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
	        _compressedFile = compressor.Compress(filesToCompress, new FileInfo(_targetZipFileName)).DefaultIfEmpty(new FileInfo("mock")).Single();
	    }

	    [TestMethod]
	    public void ZipMultipleFiles()
	    {
	        ICompressor compressor = new Zip();
	        List<FileInfo> filesToCompress = new List<FileInfo>
	        {
	            new FileInfo(_testFilePath),
	            new FileInfo(Path.Combine(_workingDirectory.FullName,"TestData.xml"))
	        };

	        _compressedFile = compressor.Compress(filesToCompress, new FileInfo(Path.Combine(_workingDirectory.FullName,_targetZipFileName))).DefaultIfEmpty(new FileInfo("mock")).Single();

            Assert.AreEqual(Path.GetExtension(_compressedFile.FullName), ".zip");
	        Assert.IsTrue(File.Exists(_compressedFile.FullName));

	        using (ZipArchive zip = ZipFile.Open(_compressedFile.FullName, ZipArchiveMode.Read))
	        {
	            ReadOnlyCollection<ZipArchiveEntry> zipEntries = zip.Entries;
	            Assert.IsTrue(zipEntries.Count == filesToCompress.Count);

	            foreach (FileInfo file in filesToCompress)
	            {
	                Assert.IsTrue(zipEntries.Any(x => x.Name == file.Name));
	            }
            }
	    }
    }
}
