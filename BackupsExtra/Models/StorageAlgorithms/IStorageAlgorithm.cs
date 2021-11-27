using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using BackupsExtra.Models.RestorePoints;

namespace BackupsExtra.Models.StorageAlgorithms
{
    public interface IStorageAlgorithm
    {
        public RestorePoint Restore(string path, IReadOnlyList<FileDescription> files, DateTime time);
    }
}