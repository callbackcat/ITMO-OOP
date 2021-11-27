using System;
using System.Collections.Generic;
using BackupsExtra.Models;

namespace BackupsExtra.Comparers
{
    public class FilesEqualityComparer : IEqualityComparer<FileDescription>
    {
        public bool Equals(FileDescription x, FileDescription y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.GetType() == y.GetType() && x.Name == y.Name;
        }

        public int GetHashCode(FileDescription obj)
            => HashCode.Combine(obj.DirectoryName, obj.Name);
    }
}