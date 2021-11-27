using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models.CleaningAlgorithms
{
    public class RestoresDateAlgorithm : ICleaningAlgorithm
    {
        public RestoresDateAlgorithm(DateTime edgeTime)
        {
            EdgeTime = edgeTime;

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public DateTime EdgeTime { get; }

        public int CountPointsToBeCleaned(IReadOnlyList<RestorePoint> restorePoints)
        {
            _ = restorePoints ?? throw new BackupsExtraException("Invalid restore points reference");

            return restorePoints.Any(p => p.CreationTime < EdgeTime)
                ? restorePoints.Count(p => p.CreationTime < EdgeTime)
                : 0;
        }

        public override string ToString() => $"{GetType().Name} with {EdgeTime} edge date";
    }
}