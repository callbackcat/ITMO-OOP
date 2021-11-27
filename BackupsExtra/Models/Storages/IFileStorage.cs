using System.Collections.Generic;
using System.IO;

namespace BackupsExtra.Models.Storages
{
    public interface IFileStorage
    {
        void AddFile(FileDescription file);
        void RemoveFile(FileDescription file);
    }
}