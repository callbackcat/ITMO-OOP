using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BackupsExtra.Comparers;
using BackupsExtra.Enums;
using BackupsExtra.Models;
using BackupsExtra.Models.CleaningAlgorithms;
using BackupsExtra.Models.FilesRestores;
using BackupsExtra.Models.Merges;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.StorageAlgorithms;
using BackupsExtra.Models.Storages;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTest
    {
        private List<FileDescription> _files;

        [SetUp]
        public void Setup()
        {
            var fileA = new FileInfo(@$"{Directory.GetCurrentDirectory()}\FileA.txt");
            var fileB = new FileInfo(@$"{Directory.GetCurrentDirectory()}\FileB.txt");
            var fileC = new FileInfo(@$"{Directory.GetCurrentDirectory()}\FileC.txt");
            var fileD = new FileInfo(@$"{Directory.GetCurrentDirectory()}\FileD.txt");

            _files = new List<FileDescription>
            {
                new FileDescription(fileA.FullName, fileA.Name, fileA.DirectoryName, fileA.Extension),
                new FileDescription(fileB.FullName, fileB.Name, fileB.DirectoryName, fileB.Extension),
                new FileDescription(fileC.FullName, fileC.Name, fileC.DirectoryName, fileC.Extension),
                new FileDescription(fileD.FullName, fileD.Name, fileD.DirectoryName, fileD.Extension),
            };
        }

        [Test]
        public void RestoresCountAlgorithmTest()
        {
            const int pointsLimit = 2;
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SplitStorageAlgorithm();
            ICleaningAlgorithm cleaningAlgorithm = new RestoresCountAlgorithm(pointsLimit);
            var backupJob = new BackupJob(directory, algorithm, cleaningAlgorithm);

            backupJob.AddFile(_files[0]);
            backupJob.AddFile(_files[1]);

            DateTime creationTime = DateTime.Now;
            backupJob.CreateRestorePoint(creationTime);
            backupJob.CreateRestorePoint(creationTime);
            backupJob.CreateRestorePoint(creationTime);
            backupJob.CleanRestorePoints();

            Assert.AreEqual(2, backupJob.Backup.RestorePoints.Count);
        }

        [Test]
        public void RestoreDateAlgorithmTest()
        {
            DateTime edgeDate = DateTime.Now;
            DateTime yesterday = edgeDate.AddDays(-1);
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SplitStorageAlgorithm();
            ICleaningAlgorithm cleaningAlgorithm = new RestoresDateAlgorithm(edgeDate);
            var backupJob = new BackupJob(directory, algorithm, cleaningAlgorithm);

            backupJob.AddFile(_files[0]);
            backupJob.AddFile(_files[1]);

            backupJob.CreateRestorePoint(yesterday);
            backupJob.CreateRestorePoint(yesterday);
            backupJob.CreateRestorePoint(edgeDate);
            backupJob.CleanRestorePoints();

            Assert.AreEqual(1, backupJob.Backup.RestorePoints.Count);
        }

        [Test]
        public void RestoreHybridAlgorithmTest_OneLimit()
        {
            DateTime edgeDate = DateTime.Now;
            DateTime yesterday = edgeDate.AddDays(-1);
            const int pointsLimit = 1;
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SplitStorageAlgorithm();

            ICleaningAlgorithm cleaningAlgorithmDate
                = new RestoresDateAlgorithm(edgeDate);

            ICleaningAlgorithm cleaningAlgorithmCount
                = new RestoresCountAlgorithm(pointsLimit);

            var cleaningAlgorithms = new List<ICleaningAlgorithm>
            {
                cleaningAlgorithmCount, cleaningAlgorithmDate,
            };

            ICleaningAlgorithm hybridCleaningAlgorithm
                = new RestoresHybridAlgorithm(cleaningAlgorithms, HybridType.OneLimit);

            var backupJob = new BackupJob(directory, algorithm, hybridCleaningAlgorithm);

            backupJob.AddFile(_files[0]);
            backupJob.AddFile(_files[1]);

            // One point should be deleted by DateAlgorithm, two points by CountAlgorithm
            // ===> must be one restore point remaining
            backupJob.CreateRestorePoint(yesterday);
            backupJob.CreateRestorePoint(edgeDate);
            backupJob.CreateRestorePoint(edgeDate);
            backupJob.CleanRestorePoints();

            Console.WriteLine(hybridCleaningAlgorithm.CountPointsToBeCleaned(backupJob.Backup.RestorePoints));

            Assert.AreEqual(1, backupJob.Backup.RestorePoints.Count);
        }

        [Test]
        public void RestoreHybridAlgorithmTest_AllLimits()
        {
            DateTime edgeDate = DateTime.Now;
            DateTime yesterday = edgeDate.AddDays(-1);
            const int pointsLimit = 1;
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SplitStorageAlgorithm();

            ICleaningAlgorithm cleaningAlgorithmDate
                = new RestoresDateAlgorithm(edgeDate);

            ICleaningAlgorithm cleaningAlgorithmCount
                = new RestoresCountAlgorithm(pointsLimit);

            var cleaningAlgorithms = new List<ICleaningAlgorithm>
            {
                cleaningAlgorithmCount, cleaningAlgorithmDate,
            };

            ICleaningAlgorithm hybridCleaningAlgorithm
                = new RestoresHybridAlgorithm(cleaningAlgorithms, HybridType.AllLimits);

            var backupJob = new BackupJob(directory, algorithm, hybridCleaningAlgorithm);

            backupJob.AddFile(_files[0]);
            backupJob.AddFile(_files[1]);

            // Two points should be deleted by DateAlgorithm, three points by CountAlgorithm
            // ===> must be two restore point remaining
            backupJob.CreateRestorePoint(yesterday);
            backupJob.CreateRestorePoint(yesterday);
            backupJob.CreateRestorePoint(edgeDate);
            backupJob.CreateRestorePoint(edgeDate);
            backupJob.CleanRestorePoints();

            Console.WriteLine(hybridCleaningAlgorithm.CountPointsToBeCleaned(backupJob.Backup.RestorePoints));

            Assert.AreEqual(2, backupJob.Backup.RestorePoints.Count);
        }

        [Test]
        public void RestorePointsMergeTest()
        {
            const int pointsLimit = 3;
            string directory = _files[0].DirectoryName;

            IStorageAlgorithm algorithm = new SplitStorageAlgorithm();
            ICleaningAlgorithm cleaningAlgorithm = new RestoresCountAlgorithm(pointsLimit);
            var backupJob = new BackupJob(directory, algorithm, cleaningAlgorithm);
            DateTime creationTime = DateTime.Now;

            backupJob.AddFile(_files[0]);
            RestorePoint a = backupJob.CreateRestorePoint(creationTime);

            backupJob.RemoveFile(_files[0]);
            backupJob.AddFile(_files[1]);
            RestorePoint b = backupJob.CreateRestorePoint(creationTime);

            backupJob.RemoveFile(_files[1]);
            backupJob.AddFile(_files[2]);
            RestorePoint c = backupJob.CreateRestorePoint(creationTime);

            backupJob.RemoveFile(_files[2]);
            backupJob.AddFile(_files[3]);
            RestorePoint d = backupJob.CreateRestorePoint(creationTime);

            backupJob.RemoveFile(_files[3]);
            backupJob.AddFile(_files[0]);
            RestorePoint newA = backupJob.CreateRestorePoint(creationTime);

            var oldPoint = new List<RestorePoint> { a, b, c };
            var newPoint = new List<RestorePoint> { newA, d };

            new RestoresMerge(oldPoint, newPoint).MergeRestorePoints(directory);
            Assert.AreEqual(5, backupJob.Backup.RestorePoints.Count);
        }
    }
}