using FileOps.Common;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Processors.Compression
{
	internal interface ICompressor
	{
        OneOrZeroElementCollection<FileInfo> Compress(IEnumerable<FileInfo> filesToCompress, FileInfo compressedFileName);

	    IEnumerable<FileInfo> Decompress(FileInfo compressedFile, DirectoryInfo decompressionDirectory = null);
	}
}
