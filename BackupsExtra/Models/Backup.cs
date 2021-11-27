using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Models.RestorePoints;
using BackupsExtra.Tools.Exceptions;

namespace BackupsExtra.Models
{
    public class Backup
    {
        private List<RestorePoint> _restorePoints;

        public Backup()
        {
            _restorePoints = new List<RestorePoint>();
        }

        public IReadOnlyList<RestorePoint> RestorePoints
        {
            get => _restorePoints;
            internal set => _restorePoints = value.ToList();
        }

        public void AddRestorePoint(RestorePoint point)
        {
            _ = point ?? throw new BackupsExtraException("Invalid restore point reference");
            _restorePoints.Add(point);
        }
    }
}