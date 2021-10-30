using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Models.Storages;

namespace Backups.Models.Repositories
{
    public sealed class WindowsRepository : IRepository
    {
        public Storage CreateSingleStorage(string path, IReadOnlyList<FileInfo> files, DateTime time)
        {
            var storage = new Storage();
            string newPath = $"{path}\\RestorePoint_{time:dd/MM/yyyy_HH-mm-ss}.zip";

            using ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Create);
            {
                foreach (FileInfo file in files)
                {
                    // Add a second to avoid file's name collision
                    time = time.AddSeconds(1);

                    string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}" +
                                         $"_backedup{time:dd/MM/yyyy_HH-mm-ss}{file.Extension}";

                    storage.AddFile(new FileInfo($"{newPath}\\{newFileName}"));

                    archive.CreateEntryFromFile(file.FullName, newFileName);
                }
            }

            return storage;
        }

        public List<Storage> CreateSplitStorages(string path, IReadOnlyList<FileInfo> files, DateTime time)
        {
            var storages = new List<Storage>();

            foreach (FileInfo file in files)
            {
                // Add a second to avoid file's name collision
                time = time.AddSeconds(1);

                string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}" +
                                     $"_backedup{time:dd/MM/yyyy_HH-mm-ss}";

                string newPath = $"{path}\\{newFileName}.zip";

                var storage = new Storage();
                storage.AddFile(new FileInfo(newPath));
                storages.Add(storage);

                using ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Create);
                archive.CreateEntryFromFile(file.FullName, $"{newFileName}{file.Extension}");
            }

            return storages;
        }
    }
}