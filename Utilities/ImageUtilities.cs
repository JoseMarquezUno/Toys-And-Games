using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class ImageUtilities
    {
        public static List<string> GetImagePathsFromAssembly(string assemblyDir, IList<string> relPaths)
        {
            List<string> paths = new();
            foreach (var item in relPaths)
            {
                paths.Add(Path.Combine(assemblyDir, item)); 
            }
            return paths;
        }

        public static List<string> GetImagesInBase64(IList<string> relPaths)
        {
            List<string> imagesBase64 = new();
            foreach (string path in relPaths)
            {
                byte[] bytes = File.ReadAllBytes(path);
                string imageBase64 = Convert.ToBase64String(bytes);
                imagesBase64.Add(imageBase64);
            }
            return imagesBase64;
        }
    }
}