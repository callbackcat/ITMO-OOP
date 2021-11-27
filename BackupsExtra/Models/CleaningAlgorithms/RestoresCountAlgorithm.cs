using System.Collections.Generic;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Tools;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models.CleaningAlgorithms
{
    public class RestoresCountAlgorithm : ICleaningAlgorithm
    {
        public RestoresCountAlgorithm(int pointsCount)
        {
            CheckPointsCount(pointsCount);
            PointsCount = pointsCount;

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public int PointsCount { get; }

        public int CountPointsToBeCleaned(IReadOnlyList<RestorePoint> restorePoints)
        {
            _ = restorePoints ?? throw new BackupsExtraException("Invalid restore points reference");

            return restorePoints.Count - PointsCount > 0
                ? restorePoints.Count - PointsCount
                : 0;
        }

        public override string ToString() => $"{GetType().Name} with {PointsCount} points";

        private void CheckPointsCount(int pointsCount)
        {
            const string messageTemplate = "Restore points count must be greater than zero";

            if (pointsCount > 0) return;
            LoggerHolder.Instance.Error(new BackupsExtraException(messageTemplate), messageTemplate);
        }
    }
}