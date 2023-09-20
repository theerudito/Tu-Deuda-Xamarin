using System;
using System.IO;
using Tu_Deuda.Data;
using Tu_Deuda.Droid;
using Tu_Deuda.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Tu_Deuda.Droid
{
    public class FileService : IFileService
    {
        private string path = DatabaseConfig.ConnectionString();
        private string directoryName = "Tu Deuda";
        private string fileName = "TuDeuda.db";
        private string destinationFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;

        public void Backup()
        {
            try
            {
                string directory = Path.Combine(destinationFolder, directoryName);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string sourcePath = Path.Combine(directory, fileName);

                File.Copy(path, sourcePath, true);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool Restore()
        {
            string directory = Path.Combine(destinationFolder, directoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sourcePath = Path.Combine(directory, fileName);

            if (!File.Exists(sourcePath))
            {
                return false;
            }

            File.Copy(sourcePath, path, true);

            return true;

        }
    }
}
