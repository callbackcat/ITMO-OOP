using System;
using System.Collections.Generic;
using BackupsExtra.Models.Storages;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models.RestorePoints
{
    public class RestorePoint
    {
        public RestorePoint(List<Storage> storage, DateTime time)
        {
            BackedupFiles = storage ?? throw new BackupsExtraException("Invalid files reference");
            CreationTime = time;

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public IReadOnlyList<Storage> BackedupFiles { get; }
        public DateTime CreationTime { get; }

        public override string ToString() => $"{GetType().Name} at {CreationTime}";
    }
}