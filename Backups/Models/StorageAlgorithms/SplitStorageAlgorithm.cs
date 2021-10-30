using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Models.Repositories;
using Backups.Models.RestorePoints;
using Backups.Models.Storages;

namespace Backups.Models.StorageAlgorithms
{
    public class SplitStorageAlgorithm : IStorageAlgorithm
    {
        public RestorePoint Restore(string path, IReadOnlyList<FileInfo> files, DateTime time)
        {
            var repository = new WindowsRepository();
            List<Storage> storages = repository.CreateSplitStorages(path, files, time);

            return new RestorePoint(storages, time);
        }
    }
}