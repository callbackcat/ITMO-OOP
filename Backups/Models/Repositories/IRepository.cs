using System;
using System.Collections.Generic;
using System.IO;
using Backups.Models.Storages;

namespace Backups.Models.Repositories
{
    public interface IRepository
    {
        public Storage CreateSingleStorage(string path, IReadOnlyList<FileInfo> files, DateTime time);
        public List<Storage> CreateSplitStorages(string path, IReadOnlyList<FileInfo> files, DateTime time);
    }
}