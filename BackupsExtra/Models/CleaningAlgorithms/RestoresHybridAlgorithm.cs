using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Enums;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models.CleaningAlgorithms
{
    public class RestoresHybridAlgorithm : ICleaningAlgorithm
    {
        public RestoresHybridAlgorithm(List<ICleaningAlgorithm> cleaningAlgorithms, HybridType type)
        {
            CleaningAlgorithms = cleaningAlgorithms ??
                                 throw new BackupsExtraException("Invalid cleaning algorithms reference");

            CleaningAlgorithmType = type;

            LoggerHolder.Instance.Debug($"Created a {this}");
        }

        public IReadOnlyList<ICleaningAlgorithm> CleaningAlgorithms { get; }
        public HybridType CleaningAlgorithmType { get; }

        public int CountPointsToBeCleaned(IReadOnlyList<RestorePoint> restorePoints)
        {
            _ = restorePoints ?? throw new BackupsExtraException("Invalid restore points reference");

            return CleaningAlgorithmType switch
            {
                HybridType.OneLimit => CleaningAlgorithms
                    .Select(a => a.CountPointsToBeCleaned(restorePoints))
                    .Max(),

                HybridType.AllLimits => CleaningAlgorithms
                    .Select(a => a.CountPointsToBeCleaned(restorePoints))
                    .Min(),

                _ => throw new BackupsExtraException(
                    "Invalid hybrid cleaning algorithm",
                    new ArgumentOutOfRangeException())
            };
        }

        public override string ToString() => $"{GetType().Name} with {CleaningAlgorithmType} type " +
                                             $"and {string.Join(" ", CleaningAlgorithms)} algorithms";
    }
}