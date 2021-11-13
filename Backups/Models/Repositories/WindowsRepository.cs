using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Models.StorageAlgorithms;
using Backups.Models.Storages;

namespace Backups.Models.Repositories
{
    public sealed class WindowsRepository : IRepository
    {
        public List<Storage> CreateStorages(string path, IReadOnlyList<FileInfo> files, DateTime time,  IStorageAlgorithm storageAlgorithm)
        {
            var storage = new List<Storage>();

            switch (storageAlgorithm)
            {
                case SingleStorageAlgorithm:
                {
                    storage.Add(new Storage());
                    string newPath = $"{path}{Path.DirectorySeparatorChar}RestorePoint_{time:dd/MM/yyyy_HH-mm-ss}.zip";

                    using ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Create);
                    {
                        foreach (FileInfo file in files)
                        {
                            // Add a second to avoid file's name collision
                            time = time.AddSeconds(1);

                            string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}" +
                                                 $"_backedup{time:dd/MM/yyyy_HH-mm-ss}{file.Extension}";

                            storage[0].AddFile(new FileInfo($"{newPath}\\{newFileName}"));

                            archive.CreateEntryFromFile(file.FullName, newFileName);
                        }
                    }

                    break;
                }

                case SplitStorageAlgorithm:
                {
                    foreach (FileInfo file in files)
                    {
                        // Add a second to avoid file's name collision
                        time = time.AddSeconds(1);

                        string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}" +
                                             $"_backedup{time:dd/MM/yyyy_HH-mm-ss}";

                        string newPath = $"{path}\\{newFileName}.zip";

                        var st = new Storage();
                        st.AddFile(new FileInfo(newPath));
                        storage.Add(st);

                        using ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Create);
                        archive.CreateEntryFromFile(file.FullName, $"{newFileName}{file.Extension}");
                    }

                    break;
                }
            }

            return storage;
        }
    }
}