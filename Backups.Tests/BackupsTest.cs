using System.Collections.Generic;
using System.IO;
using Backups.Models;
using Backups.Models.StorageAlgorithms;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private List<FileInfo> _files;

        [SetUp]
        public void Setup()
        {
            _files = new List<FileInfo>
            {
                new FileInfo($@"{Directory.GetCurrentDirectory()}\FileA.txt"),
                new FileInfo($@"{Directory.GetCurrentDirectory()}\FileB.txt")
            };
        }

        [Test]
        public void SplitStorageAlgorithmTest()
        {
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SplitStorageAlgorithm();
            var backupJob = new BackupJob(directory, algorithm);

            backupJob.AddFile(_files[0]);
            backupJob.AddFile(_files[1]);
            backupJob.CreateRestorePoint();

            backupJob.RemoveFile(_files[0]);
            backupJob.CreateRestorePoint();

            Assert.AreEqual(2, backupJob.Backup.RestorePoints.Count);
            Assert.AreEqual(2, backupJob.Backup.RestorePoints[0].GetFiles.Count);
            Assert.AreEqual(1, backupJob.Backup.RestorePoints[1].GetFiles.Count);
        }

        [Test]
        public void SingleStorageAlgorithmTest()
        {
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SingleStorageAlgorithm();
            var backupJob = new BackupJob(directory, algorithm);

            backupJob.AddFile(_files[0]);
            backupJob.AddFile(_files[1]);
            backupJob.CreateRestorePoint();

            Assert.AreEqual(1, backupJob.Backup.RestorePoints.Count);
            Assert.AreEqual(2, backupJob.Backup.RestorePoints[0].GetFiles[0].GetFiles().Count);
        }
    }
}