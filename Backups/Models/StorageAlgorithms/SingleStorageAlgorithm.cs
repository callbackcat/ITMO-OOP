using System;
using System.Collections.Generic;
using System.IO;
using Backups.Models.Repositories;
using Backups.Models.RestorePoints;
using Backups.Models.Storages;

namespace Backups.Models.StorageAlgorithms
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public RestorePoint Restore(string path, IReadOnlyList<FileInfo> files, DateTime time)
        {
            var repository = new WindowsRepository();
            List<Storage> storage = repository.CreateStorages(path, files, time, this);

            return new RestorePoint(storage, time);
        }
    }
}