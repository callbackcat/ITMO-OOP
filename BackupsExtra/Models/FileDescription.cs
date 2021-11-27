using System;

namespace BackupsExtra.Models
{
    public class FileDescription : IEquatable<FileDescription>
    {
        public FileDescription(string fullName, string name, string directoryName, string extension)
        {
            FullName = fullName;
            Name = name;
            DirectoryName = directoryName;
            Extension = extension;
        }

        public string FullName { get; }
        public string Name { get; }
        public string DirectoryName { get; }
        public string Extension { get; }

        public bool Equals(FileDescription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FileDescription)obj);
        }

        public override int GetHashCode() => HashCode.Combine(FullName, Name, DirectoryName, Extension);
        public override string ToString() => Name;
    }
}