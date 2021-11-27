using System.Collections.Generic;
using BackupsExtra.Models.RestorePoints;

namespace BackupsExtra.Models.CleaningAlgorithms
{
    public interface ICleaningAlgorithm
    {
        int CountPointsToBeCleaned(IReadOnlyList<RestorePoint> restorePoints);
    }
}