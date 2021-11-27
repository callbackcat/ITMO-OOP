using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BackupsExtra.Enums;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.Storages;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models.FilesRestores
{
    public class WindowsFileRestore : IFileRestorer
    {
        private readonly RestorePoint _restorePoint;
        private readonly FileRestoreType _fileRestoreType;

        public WindowsFileRestore(RestorePoint restorePoint, FileRestoreType type)
        {
            _restorePoint = restorePoint
                            ?? throw new BackupsExtraException("Invalid restore point reference");

            _fileRestoreType = type;

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public void Restore(string location = null)
        {
            switch (_fileRestoreType)
            {
                case FileRestoreType.Original:
                    var storagesOriginal = _restorePoint
                        .BackedupFiles
                        .Select(s => s)
                        .ToList();

                    foreach (Storage storage in storagesOriginal)
                    {
                        string directory = storage.Path;
                        foreach (FileDescription file in storage.Files)
                        {
                            ZipFile.ExtractToDirectory(
                                @$"{directory}",
                                $"{file.FullName}_restored");
                        }
                    }

                    break;
                case FileRestoreType.Different:
                    if (string.IsNullOrWhiteSpace(location))
                        throw new BackupsExtraException("Location must be valid");

                    var storagesDifferent = _restorePoint
                        .BackedupFiles
                        .Select(s => s)
                        .ToList();

                    foreach (Storage storage in storagesDifferent)
                    {
                        string directory = storage.Path;
                        foreach (FileDescription file in storage.Files)
                        {
                            ZipFile.ExtractToDirectory(
                                @$"{directory}",
                                location);
                        }
                    }

                    break;
                default:
                    throw new BackupsExtraException(
                        "Invalid file restore type",
                        new ArgumentOutOfRangeException());
            }
        }

        public override string ToString() => $"{GetType().Name} with {_fileRestoreType} restore type";
    }
}