using System.Collections.Generic;
using System.IO;

namespace Backups.Models.Storages
{
    public abstract class FileStorage
    {
        protected FileStorage()
        {
            Files = new List<FileInfo>();
        }

        protected List<FileInfo> Files { get; }

        public IReadOnlyList<FileInfo> GetFiles() => Files;
        public abstract void AddFile(FileInfo file);
        public abstract void RemoveFile(FileInfo file);
    }
}