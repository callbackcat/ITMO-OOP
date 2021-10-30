using System.Collections.Generic;
using Backups.Models.RestorePoints;
using Backups.Tools;

namespace Backups.Models
{
    public class Backup
    {
        private readonly List<RestorePoint> _restorePoints;

        public Backup()
        {
            _restorePoints = new List<RestorePoint>();
        }

        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public void AddRestorePoint(RestorePoint point)
        {
            _ = point ?? throw new BackupsException("Invalid restore point reference");
            _restorePoints.Add(point);
        }
    }
}