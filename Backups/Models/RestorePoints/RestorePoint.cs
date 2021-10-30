using System;
using System.Collections.Generic;
using Backups.Models.Storages;
using Backups.Tools;

namespace Backups.Models.RestorePoints
{
    public class RestorePoint
    {
        private readonly List<Storage> _backedupFiles;

        public RestorePoint(List<Storage> storage, DateTime time)
        {
            _backedupFiles = storage ?? throw new BackupsException("Invalid files reference");
            CreationTime = time;
        }

        public IReadOnlyList<Storage> GetFiles => _backedupFiles;

        internal DateTime CreationTime { get; }
    }
}