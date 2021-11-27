using System.IO;
using BackupsExtra.Tools.Exceptions;
using Newtonsoft.Json;

namespace BackupsExtra.Models
{
    public static class StateHelper
    {
        public static void AddJob(BackupJob job)
        {
            _ = job ?? throw new BackupsExtraException("Invalid job reference");
        }

        public static void SaveProgramState(string path, BackupJob job)
        {
            string json = JsonConvert.SerializeObject(job, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });

            File.WriteAllText(path, json);
        }

        public static BackupJob GetProgramState(string jsonPath)
        {
            string json = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<BackupJob>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }
    }
}