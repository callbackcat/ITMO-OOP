namespace BackupsExtra.Models.FilesRestores
{
    public interface IFileRestorer
    {
        void Restore(string location = null);
    }
}