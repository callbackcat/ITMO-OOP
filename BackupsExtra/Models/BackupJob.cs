using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Models.CleaningAlgorithms;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.StorageAlgorithms;
using BackupsExtra.Tools.Exceptions;

namespace BackupsExtra.Models
{
    public class BackupJob
    {
        public BackupJob(string path, IStorageAlgorithm storageAlgorithm, ICleaningAlgorithm cleaningAlgorithm)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new BackupsExtraException("Invalid path reference");

            StorageAlgorithm = storageAlgorithm
                               ?? throw new BackupsExtraException("Invalid algorithm reference");

            CleaningAlgorithm = cleaningAlgorithm
                                ?? throw new BackupsExtraException("Invalid cleaning algorithm reference");

            Path = path;
            Backup = new Backup();
            JobObjects = new List<JobObject>();
        }

        public IStorageAlgorithm StorageAlgorithm { get; }
        public ICleaningAlgorithm CleaningAlgorithm { get; }
        public List<JobObject> JobObjects { get; }
        public Backup Backup { get; }
        public string Path { get; }

        public void AddFile(FileDescription file) => JobObjects.Add(new JobObject(file));

        public void RemoveFile(FileDescription file)
        {
            JobObject jobObject = JobObjects.FirstOrDefault(j => j.File == file);
            _ = jobObject ?? throw new BackupsExtraException("The job with file:" +
                                                             $"{file.Name} wasn't found");

            JobObjects.Remove(jobObject);
        }

        public RestorePoint CreateRestorePoint(DateTime time)
        {
            var jobsFiles = JobObjects
                .Select(j => j.File)
                .ToList();

            RestorePoint restorePoint = StorageAlgorithm
                .Restore(Path, jobsFiles, time);

            Backup.AddRestorePoint(restorePoint);

            return restorePoint;
        }

        public void CleanRestorePoints()
        {
            int pointsCountToBeCleaned = CleaningAlgorithm
                .CountPointsToBeCleaned(Backup.RestorePoints);

            var restorePointsToRemove = Backup.RestorePoints
                .OrderBy(r => r.CreationTime)
                .Take(pointsCountToBeCleaned)
                .ToList();

            restorePointsToRemove
                .SelectMany(r => r.BackedupFiles)
                .ToList()
                .ForEach(s => File.Delete(s.Path));

            Backup.RestorePoints = Backup.RestorePoints.Skip(pointsCountToBeCleaned).ToList();
        }
    }
}