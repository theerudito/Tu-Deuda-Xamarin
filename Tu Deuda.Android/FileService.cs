using System.IO;
using Tu_Deuda.Droid;
using Tu_Deuda.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Tu_Deuda.Droid
{
    public class FileService : IFileService
    {
        public void CopyFile(string sourceFilePath, string fileName)
        {
            string destinationDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath; ;
            string destinationPath = Path.Combine(destinationDirectory, fileName);
            File.Copy(sourceFilePath, destinationPath, true);
        }

        public string GetPath(string fileName)
        {


            string destinationDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath; ;
            string destinationPath = Path.Combine(destinationDirectory, fileName);
            return destinationPath;
        }
    }
}