using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Models.RestorePoints;
using Backups.Models.StorageAlgorithms;
using Backups.Tools;

namespace Backups.Models
{
    public class BackupJob
    {
        private readonly List<JobObject> _jobObjects;
        private readonly IStorageAlgorithm _storageAlgorithm;

        public BackupJob(string path, IStorageAlgorithm algorithm)
        {
            _storageAlgorithm = algorithm ?? throw new BackupsException("Invalid algorithm reference");

            if (string.IsNullOrWhiteSpace(path))
                throw new BackupsException("Invalid path reference");

            Path = path;
            Backup = new Backup();
            _jobObjects = new List<JobObject>();
        }

        public Backup Backup { get; }
        private string Path { get; }
        public void AddFile(FileInfo file) => _jobObjects.Add(new JobObject(file));

        public void RemoveFile(FileInfo file)
        {
            JobObject jobObject = _jobObjects.FirstOrDefault(j => j.File == file);
            _ = jobObject ?? throw new BackupsException($"The job with file: {file.Name} wasn't found");

            _jobObjects.Remove(jobObject);
        }

        public RestorePoint CreateRestorePoint()
        {
            var jobsFiles = _jobObjects
                .Select(j => j.File)
                .ToList();

            RestorePoint restorePoint = _storageAlgorithm
                .Restore(Path, jobsFiles, DateTime.Now);

            Backup.AddRestorePoint(restorePoint);
            return restorePoint;
        }
    }
}