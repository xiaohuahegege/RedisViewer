using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RedisViewer.Core
{
    public static class FileExtensions
    {
        /// <summary>
        /// Ensure create directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string EnsureCreateDirectory(this string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var dir = new FileInfo(path).Directory.FullName;

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }
            }
            catch
            {
                // ignore exception
            }

            return path;
        }

        public static async Task<string> ReadJsonFromFileAsync(this string path, Encoding encoding)
        {
            try
            {
                return await File.ReadAllTextAsync(path, encoding);
            }
            catch
            {

            }

            return null;
        }

        public static async Task<bool> WriteJsonToFileAsync(this string path, string content, Encoding encoding)
        {
            try
            {
                await File.WriteAllTextAsync(path, content, encoding);
                return true;
            }
            catch
            {

            }

            return false;
        }
    }
}