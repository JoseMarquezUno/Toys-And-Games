using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class ImageUtilities
    {
        
        public static List<string> GetImagePathsFromAssembly(string assemblyDirectory, IList<string> relativePaths)
        {
            List<string> paths = new();
            foreach (var item in relativePaths)
            {
                paths.Add(Path.Combine(assemblyDirectory, item)); 
            }
            return paths;
        }

        public static List<string> GetImagesInBase64(IList<string> paths)
        {
            List<string> imagesBase64 = new();
            try
            {
                foreach (string path in paths)
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    string imageBase64 = Convert.ToBase64String(bytes);
                    imagesBase64.Add(imageBase64);
                }
                return imagesBase64;
            }
            catch (Exception)
            {
                return imagesBase64;
            }
        }
        public static string GenerateImageName()
        {
            //TODO: Same,. use string interpolation
            return $"img_{DateTime.Now.Ticks}";
        }
    }
}