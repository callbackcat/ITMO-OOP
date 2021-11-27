using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Models.Storages;

namespace BackupsExtra.Comparers
{
    public class StoragesEqualityComparer : IEqualityComparer<Storage>
    {
        public bool Equals(Storage x, Storage y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.GetType() == y.GetType() && x.Files.SequenceEqual(y.Files, new FilesEqualityComparer());
        }

        public int GetHashCode(Storage obj)
            => HashCode.Combine(obj.Path, obj.Files);
    }
}