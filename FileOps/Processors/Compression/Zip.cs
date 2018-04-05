using FileOps.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FileOps.Processors.Compression
{
	internal class Zip : ICompressor
	{
		private const string DefaultExtension = ".zip";

		private readonly CompressionLevel _compressionLevel;

		public Zip(CompressionLevel compressionLevel = CompressionLevel.Fastest) => _compressionLevel = compressionLevel;

		public OneOrZeroElementCollection<FileInfo> Compress(IEnumerable<FileInfo> filesToCompress, FileInfo compressedFileName)
		{
			if (filesToCompress == null) throw new ArgumentNullException(nameof(filesToCompress));
			if (compressedFileName == null) throw new ArgumentNullException(nameof(compressedFileName));
			if (!Directory.Exists(compressedFileName.Directory.FullName)) throw new DirectoryNotFoundException(compressedFileName.Directory.FullName);

			IList<FileInfo> filesList = filesToCompress.ToList();

			if (!filesList.Any()) return new OneOrZeroElementCollection<FileInfo>();
			if (filesList.Any(x => !File.Exists(x.FullName))) throw new FileNotFoundException(filesList.First(x => !File.Exists(x.FullName)).FullName);
			if (!string.Equals(compressedFileName.Extension, DefaultExtension, StringComparison.CurrentCultureIgnoreCase))
				throw new ArgumentException($"Invalid {compressedFileName.Name} filename. Check whether {nameof(filesToCompress)} ends with {DefaultExtension}");

			using (ZipArchive zip = ZipFile.Open(compressedFileName.FullName, ZipArchiveMode.Create))
			{
				foreach (FileInfo fileToCompress in filesList)
				{
					zip.CreateEntryFromFile(fileToCompress.FullName, fileToCompress.Name, _compressionLevel);
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

			using (ZipArchive zip = ZipFile.Open(compressedFile.FullName, ZipArchiveMode.Read))
			{
				zip.ExtractToDirectory(decompressionDirectory.FullName, true);

				List<FileInfo> resultFiles = new List<FileInfo>();
				resultFiles.AddRange(zip.Entries.Select(zipArchiveEntry => new FileInfo(Path.Combine(decompressionDirectory.FullName, zipArchiveEntry.FullName))));

				return resultFiles;
			}
		}
	}
}
