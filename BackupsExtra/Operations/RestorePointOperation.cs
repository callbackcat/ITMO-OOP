using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Comparers;
using BackupsExtra.Models;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Models.Storages;

namespace BackupsExtra.Operations
{
    public static class RestorePointOperation
    {
        internal static List<FileDescription> GetFilesFromRestorePoints(List<RestorePoint> points)
            => points
                .SelectMany(GetFilesFromRestorePoint)
                .ToList();

        internal static List<FileDescription> MergeCollections(List<FileDescription> first, List<FileDescription> second)
            => first
                .Concat(second)
                .Distinct(new FilesEqualityComparer())
                .ToList();

        internal static List<FileDescription> GetFilesIntersection(List<FileDescription> intersectFrom, List<FileDescription> from)
            => intersectFrom
                .Intersect(from, new FilesEqualityComparer())
                .ToList();

        internal static List<Storage> GetStoragesIntersection(List<Storage> intersectFrom, List<Storage> from)
            => intersectFrom
                .Intersect(from, new StoragesEqualityComparer())
                .ToList();

        internal static List<Storage> GetStoragesFromRestorePoints(List<RestorePoint> points)
            => points
                .SelectMany(s => s.BackedupFiles)
                .ToList();

        private static List<FileDescription> GetFilesFromRestorePoint(RestorePoint point)
            => point
                .BackedupFiles
                .SelectMany(s => s.Files)
                .ToList();
    }
}