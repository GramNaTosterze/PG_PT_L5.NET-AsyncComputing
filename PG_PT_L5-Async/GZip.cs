using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PG_PT_L5_Async
{
    class GZip
    {
        public GZip()
        {

        }
        public static void Compress(DirectoryInfo dir)
        {
            List<Task> compressFileTaskList = new List<Task>();
            foreach (FileInfo file in dir.GetFiles())
                compressFileTaskList.Add(Task.Factory.StartNew(
                      () => Compress(file)
                    ));
            Task.WaitAll(compressFileTaskList.ToArray());
            MessageBox.Show("Compression completed");
        }
        private static void Compress(FileInfo file)
        {
            if (file.Extension == ".gz")
                return;

            byte[] bytes = File.ReadAllBytes(file.FullName);
            using (FileStream fs = new FileStream(file.FullName + ".gz", FileMode.Create))
            using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
                zipStream.Write(bytes, 0, bytes.Length);
        }

        public static void Decompress(DirectoryInfo dir)
        {
            List<Task> decompressFileTaskList = new List<Task>();
            foreach (FileInfo file in dir.GetFiles())
                decompressFileTaskList.Add(Task.Factory.StartNew(
                    () => Decompress(file)
                    ));
            Task.WaitAll(decompressFileTaskList.ToArray());
            MessageBox.Show("Decompression completed");
        }
        private static void Decompress(FileInfo file)
        {
            if (file.Extension != ".gz")
                return;

            using (FileStream zipFile = new FileStream(file.FullName, FileMode.Open))
            using (FileStream fs = new FileStream(file.FullName.Remove(file.FullName.Length - file.Extension.Length), FileMode.Create))
            using (GZipStream uzipStream = new GZipStream(zipFile, CompressionMode.Decompress, false))
                uzipStream.CopyTo(fs);

        }
    }
}
