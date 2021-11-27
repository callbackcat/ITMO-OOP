using System;
using System.Collections.Generic;
using BackupsExtra.Models.Repositories;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.Storages;

namespace BackupsExtra.Models.StorageAlgorithms
{
    public class SplitStorageAlgorithm : IStorageAlgorithm
    {
        public RestorePoint Restore(string path, IReadOnlyList<FileDescription> files, DateTime time)
        {
            var repository = new WindowsRepository();
            List<Storage> storages = repository.CreateStorages(path, files, time, this);

            return new RestorePoint(storages, time);
        }
    }
}