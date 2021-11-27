using System;
using System.Collections.Generic;
using BackupsExtra.Models.Repositories;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.Storages;

namespace BackupsExtra.Models.StorageAlgorithms
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public RestorePoint Restore(string path, IReadOnlyList<FileDescription> files, DateTime time)
        {
            var repository = new WindowsRepository();
            List<Storage> storage = repository.CreateStorages(path, files, time, this);

            return new RestorePoint(storage, time);
        }
    }
}