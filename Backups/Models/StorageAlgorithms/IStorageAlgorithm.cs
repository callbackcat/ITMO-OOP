using System;
using System.Collections.Generic;
using System.IO;
using Backups.Models.RestorePoints;

namespace Backups.Models.StorageAlgorithms
{
    public interface IStorageAlgorithm
    {
        public RestorePoint Restore(string path, IReadOnlyList<FileInfo> files, DateTime time);
    }
}