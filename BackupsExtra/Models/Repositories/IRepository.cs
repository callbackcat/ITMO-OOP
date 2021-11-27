using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Models.StorageAlgorithms;
using BackupsExtra.Models.Storages;

namespace BackupsExtra.Models.Repositories
{
    public interface IRepository
    {
        public List<Storage> CreateStorages(string path, IReadOnlyList<FileDescription> files, DateTime time, IStorageAlgorithm storageAlgorithm);
    }
}