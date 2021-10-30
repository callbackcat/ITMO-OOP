using System.IO;
using Backups.Tools;

namespace Backups.Models.Storages
{
    public sealed class Storage : FileStorage
    {
        public override void AddFile(FileInfo file)
        {
            _ = file ?? throw new BackupsException("Invalid file reference");
            Files.Add(file);
        }

        public override void RemoveFile(FileInfo file)
        {
            if (!Files.Contains(file))
                throw new BackupsException($"The job with file: {file.Name} wasn't found");

            Files.Remove(file);
        }
    }
}