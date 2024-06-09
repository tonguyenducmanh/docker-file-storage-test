using System.Security.Cryptography;

namespace InfoTest
{
    /// <summary>
    /// class thực hiện test việc đọc ghi file trong thư mục storage của docker
    /// </summary>
    public class DirectoryInfoTest
    {
        /// <summary>
        /// root path của container
        /// </summary>
        private string _sroucePath = Environment.CurrentDirectory;

        /// <summary>
        /// thư mục sẽ tạo
        /// </summary>
        private string _folderName = "Storage";

        public DirectoryInfoTest() { }

        /// <summary>
        /// hàm chạy test
        /// </summary>
        public void RunTest()
        {
            List<string> allFileNames = new List<string>();
            string path = Path.Combine(_sroucePath, _folderName);

            // xóa các thư mục đã debug ở lần trước
            DeleteFolderIfExists(path);

            // tạo mới lại thư mục
            CreateFolderIfNotExists(path);

            // tạo ngẫu nhiên các file
            CreateRandomFiles(path, 4, 4);

            // đọc thông tin các file
            ReadFileInfo(allFileNames, path);
        }

        /// <summary>
        /// đọc toàn bộ thông tin file
        /// </summary>
        /// <param name="allFileNames"></param>
        /// <param name="path"></param>
        private void ReadFileInfo(List<string> allFileNames, string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory?.Exists == true)
            {
                Console.WriteLine("Đã load được thư mục");
                FileInfo[] infos = directory.GetFiles();
                if (infos.Length > 0)
                {
                    Console.WriteLine("Có file trong thư mục này");
                    foreach (FileInfo currentFile in infos)
                    {
                        allFileNames.Add(currentFile.Name);
                    }
                }
            }
            if (allFileNames?.Count > 0)
            {
                Console.WriteLine(string.Join(",", allFileNames));
            }
        }

        /// <summary>
        /// tạo ra các file ngẫu nhiên trong thư mục gốc
        /// </summary>
        /// <param name="path">đường dẫn thư mục gốc</param>
        public void CreateRandomFiles(string path, int numberFile, int fileSizeInMB)
        {
            for (int i = 0; i < numberFile; i++) 
            {
                string filePath = Path.Combine(path,$"TestFile_{i + 1}.txt");
                CreateRandomFile(filePath, fileSizeInMB);
            }
        }

        /// <summary>
        /// xóa thư mục nếu tồn tại
        /// </summary>
        /// <param name="path">đường dẫn thư mục</param>
        public void DeleteFolderIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// tạo thư mục nếu chưa tồn tại
        /// </summary>
        /// <param name="path">đường dẫn thư mục</param>
        public void CreateFolderIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// tạo ra file ngẫu nhiên
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sizeInMb"></param>
        public void CreateRandomFile(string filePath, int sizeInMb)
        {
            // Note: block size must be a factor of 1MB to avoid rounding errors
            const int blockSize = 1024 * 8;
            const int blocksPerMb = (1024 * 1024) / blockSize;

            byte[] data = new byte[blockSize];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                using (FileStream stream = File.OpenWrite(filePath))
                {
                    for (int i = 0; i < sizeInMb * blocksPerMb; i++)
                    {
                        crypto.GetBytes(data);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
        }
    }
}
