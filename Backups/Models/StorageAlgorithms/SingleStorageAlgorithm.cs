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
            Storage storage = repository.CreateSingleStorage(path, files, time);

            return new RestorePoint(new List<Storage> { storage }, time);
        }
    }
}