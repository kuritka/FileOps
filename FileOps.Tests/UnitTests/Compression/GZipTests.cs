//using System;
//using System.IO;
//using FakeItEasy;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using FileOps.Configuration.Entities;
//using FileOps.Tests.Common;
//using FileOps.Processors.Compression;

//namespace Beamer.Tests.Compression
//{
//    [TestClass]
//    public class GZipTests
//    {
//        private readonly DirectoryInfo _workingDirectory = new DirectoryInfo("ZipTest");
//        private readonly ZipSettings _zipSettings = new ZipSettings();

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "zipSettings")]
//        public void GZipZipSettingsIsNull()
//        {
//            GZip unused = new GZip(null, _applicationSettings, _workingDirectory, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "applicationSettings")]
//        public void GZipApplicationSettingsIsNull()
//        {
//            GZip unused = new GZip(_zipSettings, null, _workingDirectory, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "MongoSettings")]
//        public void GZipMongoSettingsIsNull()
//        {
//            _applicationSettings.MongoSettings = null;
//            GZip unused = new GZip(_zipSettings, _applicationSettings, _workingDirectory, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "parentWorkingDirectory")]
//        public void GZipParentWorkingDirectoryIsNull()
//        {
//            GZip unused = new GZip(_zipSettings, _applicationSettings, null, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "loggerFactory")]
//        public void GZipLoggerFactoryIsNull()
//        {
//            GZip unused = new GZip(_zipSettings, _applicationSettings, _workingDirectory, null);
//        }
//    }
//}