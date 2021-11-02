using System.IO;
using Backups.Tools;

namespace Backups.Models
{
    public class JobObject
    {
        public JobObject(FileInfo file)
        {
            File = file ?? throw new BackupsException("Invalid file reference");
        }

        internal FileInfo File { get; }
    }
}