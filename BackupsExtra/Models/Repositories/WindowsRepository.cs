using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using BackupsExtra.Models.StorageAlgorithms;
using BackupsExtra.Models.Storages;

namespace BackupsExtra.Models.Repositories
{
    public class WindowsRepository : IRepository
    {
        public List<Storage> CreateStorages(string path, IReadOnlyList<FileDescription> files, DateTime time,  IStorageAlgorithm storageAlgorithm)
        {
            var storage = new List<Storage>();

            switch (storageAlgorithm)
            {
                case SingleStorageAlgorithm:
                {
                    var guid = Guid.NewGuid();
                    string newPath = $"{path}\\RestorePoint_{guid}.zip";
                    storage.Add(new Storage(newPath));

                    using ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Create);
                    {
                        foreach (FileDescription file in files)
                        {
                            string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}" +
                                                 $"_backedup_{guid}{file.Extension}";

                            var info = new FileInfo($"{newPath}\\{newFileName}");

                            storage[0].AddFile(new FileDescription(info.FullName, info.Name, info.DirectoryName, info.Extension));

                            archive.CreateEntryFromFile(file.FullName, file.Name!);
                        }
                    }

                    break;
                }

                case SplitStorageAlgorithm:
                {
                    foreach (FileDescription file in files)
                    {
                        string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}" +
                                             $"_backedup_{Guid.NewGuid()}";

                        string newPath = $"{path}\\{newFileName}.zip";

                        var st = new Storage(newPath);
                        st.AddFile(new FileDescription(file.FullName, file.Name, file.DirectoryName, file.Extension));
                        storage.Add(st);

                        using ZipArchive archive = ZipFile.Open(newPath, ZipArchiveMode.Create);
                        archive.CreateEntryFromFile(file.FullName, $"{file.Name}{file.Extension}");
                    }

                    break;
                }
            }

            return storage;
        }
    }
}