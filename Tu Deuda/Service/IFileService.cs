namespace Tu_Deuda.Service
{
    public interface IFileService
    {
        void CopyFile(string sourceFilePath, string fileName);
        string GetPath(string fileName);
    }
}
