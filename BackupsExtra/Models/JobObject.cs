using BackupsExtra.Tools.Exceptions;
using BackupsExtra.Tools.Loggers;

namespace BackupsExtra.Models
{
    public class JobObject
    {
        public JobObject(FileDescription file)
        {
            File = file ?? throw new BackupsExtraException("Invalid file reference");

            LoggerHolder.Instance.Debug($"Created a job object of {file.Name}");
        }

        public FileDescription File { get; }
    }
}