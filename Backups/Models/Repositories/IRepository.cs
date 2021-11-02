using System;
using System.Collections.Generic;
using System.IO;
using Backups.Models.StorageAlgorithms;
using Backups.Models.Storages;

namespace Backups.Models.Repositories
{
    public interface IRepository
    {
        public List<Storage> CreateStorages(string path, IReadOnlyList<FileInfo> files, DateTime time, IStorageAlgorithm storageAlgorithm);
    }
}