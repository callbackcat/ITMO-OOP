using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Comparers;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.Storages;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models.Merges
{
    public class RestoresMerge
    {
        private readonly List<RestorePoint> _oldRestorePoints;
        private readonly List<RestorePoint> _newRestorePoints;

        public RestoresMerge(List<RestorePoint> oldRestorePoints, List<RestorePoint> newRestorePoints)
        {
            _oldRestorePoints = oldRestorePoints
                                ?? throw new BackupsExtraException("Invalid old restore points reference");

            _newRestorePoints = newRestorePoints
                                ?? throw new BackupsExtraException("Invalid new restore points reference");

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public RestorePoint MergeRestorePoints(string storagePath)
        {
            var oldStorages = _oldRestorePoints
                .SelectMany(s => s.BackedupFiles).ToList();

            var newStorages = _newRestorePoints
                .SelectMany(s => s.BackedupFiles).ToList();

            var k = newStorages
                .Intersect(oldStorages, new StoragesEqualityComparer())
                .Distinct()
                .ToList();

            k.ToList().ForEach(s => File.Delete(s.Path));

            var newRestorePointStorages = new List<Storage>();

            return new RestorePoint(newRestorePointStorages, DateTime.Now);
        }

        public override string ToString() => $"{GetType().Name} operation with {_oldRestorePoints.Count} old" +
                                             $"restore points and {_newRestorePoints.Count} restore points";
    }
}