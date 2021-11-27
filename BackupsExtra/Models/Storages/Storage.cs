using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;
using Serilog.Core;

namespace BackupsExtra.Models.Storages
{
    public class Storage : IFileStorage, IEquatable<Storage>
    {
        public Storage(string path)
        {
            Path = path;
            Files = new List<FileDescription>();

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public string Path { get; }
        public List<FileDescription> Files { get; }

        public void AddFile(FileDescription file)
        {
            _ = file ?? throw new BackupsExtraException("Invalid file reference");
            Files.Add(file);
        }

        public void RemoveFile(FileDescription file)
        {
            if (!Files.Contains(file))
                throw new BackupsExtraException($"The job with file: {file.Name} wasn't found");

            Files.Remove(file);
        }

        public bool Equals(Storage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Files.SequenceEqual(other.Files);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Storage)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Files);
        }

        public override string ToString() => $"{GetType().Name} in {Path} with {Files.Count} files";
    }
}