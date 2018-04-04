using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;
using FileOps.Processors.Compression;
using FileOps.Common;

namespace Beamer.Infrastructure.Compression
{
    internal class PasswordProtectedZip : ICompressor
    {
        
        private readonly CompressionLevel _compressionLevel;

        private readonly string _password;

        public PasswordProtectedZip(string password, System.IO.Compression.CompressionLevel compressionLevel = System.IO.Compression.CompressionLevel.Fastest)
        {
            if(string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            _password = password;
            _compressionLevel = compressionLevel.AsIonicCompressionLevel();
            
        }


        public OneOrZeroElementCollection<FileInfo> Compress(IEnumerable<FileInfo> filesToCompress, FileInfo compressedFileName)
        {
            if (filesToCompress == null) throw new ArgumentNullException(nameof(filesToCompress));

            if (compressedFileName == null) throw new ArgumentNullException(nameof(compressedFileName));

            if (!Directory.Exists(compressedFileName.Directory.FullName)) throw new DirectoryNotFoundException(compressedFileName.Directory.FullName);

            IList<FileInfo> filesList = filesToCompress.ToList();

            if (!filesList.Any()) return new OneOrZeroElementCollection<FileInfo>();

            Array.ForEach(filesList.ToArray(), f => f.ThrowExceptionIfNullOrDoesntExists());

            compressedFileName.ThrowExceptionIfExtensionIsDifferentFrom(Constants.FileExtensions.Zip);

            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (ZipFile zip = new ZipFile(compressedFileName.FullName))
            {
                IEnumerable<string> filestoZip = filesList.Select(f => f.FullName);
                zip.AddFiles(filestoZip, string.Empty);
                Array.ForEach(zip.Entries.ToArray(),
                    d =>
                    {
                        d.Password = _password;
                        d.CompressionLevel = _compressionLevel;
                        //todo: I cannot extract file encrypted with AES256
                        d.Encryption = EncryptionAlgorithm.Zip20;
                    }
                );

                zip.Save();
            }
            return new OneOrZeroElementCollection<FileInfo>(compressedFileName);
        }

        public IEnumerable<FileInfo> Decompress(FileInfo compressedFile, DirectoryInfo decompressionDirectory = null)
        {
            throw new NotImplementedException();
        }


    }
}
