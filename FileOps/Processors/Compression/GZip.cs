using FileOps.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FileOps.Processors.Compression
{
	internal class GZip : ICompressor
	{
		private const string DefaultExtension = ".gz";

		private readonly CompressionLevel _compressionLevel;
	    private readonly string _decompressedFileExtension;


	    public GZip(string decompressedFileExtension = null, CompressionLevel compressionLevel = CompressionLevel.Fastest)
	    {
	        _compressionLevel = compressionLevel;
	        _decompressedFileExtension = decompressedFileExtension;
	    }



		public OneOrZeroElementCollection<FileInfo> Compress(IEnumerable<FileInfo> filesToCompress, FileInfo compressedFileName)
		{
			if (filesToCompress == null) throw new ArgumentNullException(nameof(filesToCompress));
			if (compressedFileName == null) throw new ArgumentNullException(nameof(compressedFileName));
			if (!Directory.Exists(compressedFileName.Directory.FullName)) throw new DirectoryNotFoundException(compressedFileName.Directory.FullName);

			IList<FileInfo> filesList = filesToCompress.ToList();

			if (filesList.Count > 1)
				throw new ArgumentException($"Invalid {nameof(filesToCompress)} List {nameof(filesToCompress)} contains more than one file. GZip does not support compression of multiple files to one GZip archive.");

			FileInfo fileToCompress = filesList.FirstOrDefault();

			if (fileToCompress == null) throw new ArgumentException($"Invalid {nameof(filesToCompress)} List {nameof(filesToCompress)} is empty");
			if (!File.Exists(fileToCompress.FullName)) throw new FileNotFoundException(fileToCompress.FullName);
			if (!string.Equals(compressedFileName.Extension, DefaultExtension, StringComparison.CurrentCultureIgnoreCase))
				throw new ArgumentException($"Invalid {compressedFileName.Name} filename. Check whether {nameof(filesToCompress)} ends with {DefaultExtension}");

			using (Stream istream = fileToCompress.OpenRead())
			{
				using (FileStream zippedOutput = File.Create(compressedFileName.FullName))
				{
					using (GZipStream newGzipStream = new GZipStream(zippedOutput, _compressionLevel))
					{
						istream.CopyTo(newGzipStream);
					}
				}
			}
			return new OneOrZeroElementCollection<FileInfo>(compressedFileName);
		}

		public IEnumerable<FileInfo> Decompress(FileInfo compressedFile, DirectoryInfo decompressionDirectory = null)
		{
			if (compressedFile == null) throw new ArgumentNullException(nameof(compressedFile));
			if (!File.Exists(compressedFile.FullName)) throw new FileNotFoundException(compressedFile.FullName);
			if (!string.Equals(compressedFile.Extension, DefaultExtension, StringComparison.CurrentCultureIgnoreCase))
				throw new ArgumentException($"Invalid filename. Check whether {compressedFile.Name} ends with {DefaultExtension}");
			if (decompressionDirectory != null && !Directory.Exists(decompressionDirectory.FullName)) throw new DirectoryNotFoundException(decompressionDirectory.FullName);

			if (decompressionDirectory == null)
			{
				decompressionDirectory = compressedFile.Directory;
			}

			FileInfo decompressedFile = new FileInfo(Path.Combine(decompressionDirectory.FullName, $"{Path.GetFileNameWithoutExtension(compressedFile.Name)}.{_decompressedFileExtension}"));

			using (FileStream compressedStream = compressedFile.OpenRead())
			{
				using (FileStream decompressedFileStream = File.Create(decompressedFile.FullName))
				{
					using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress, false))
					{
						gzipStream.CopyTo(decompressedFileStream);
						return new List<FileInfo> { decompressedFile };
					}
				}
			}
		}
	}
}
