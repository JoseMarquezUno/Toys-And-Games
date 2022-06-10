using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Models.DTO;
using ToysAndGames.Services.Contracts;
using Utilities;

namespace ToysAndGames.Services.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly ToysAndGamesContext _context;
        public ProductImageService(ToysAndGamesContext context)
        {
            _context = context;
        }

        public void AddProductImage(IList<ProductImageDTO> productImages, int productId)
        {
            string fileName = string.Empty;
            string path = string.Empty;
            if (productImages.Count > 0)
            {
                string asmDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
                List<ProductImage> images = new();
                foreach (var image in productImages)
                {
                    fileName = ImageUtilities.GenerateImageName();
                    path = "Images\\" + fileName+ ".jpg";

                    images.Add(new()
                    {
                        ProductId = productId,
                        ImagePath = path 
                    });
                    byte[] bytes = Convert.FromBase64String(image.ImageBase64);
                    path = asmDir + "\\" + path;
                    var parentPath = Directory.GetParent(path).FullName;
                    if (Directory.Exists(parentPath))
                    {
                        File.WriteAllBytes(path, bytes); 
                    }
                    else
                    {
                        Directory.CreateDirectory(parentPath);
                        File.WriteAllBytes(path, bytes);
                    }
                }
                _context.ProductImages.AddRange(images);
                _context.SaveChanges();

            }
        }

        public void DeleteProductImage(int id)
        {
            string asmDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            ProductImage image = _context.ProductImages.FirstOrDefault(p => p.ProductImageId == id);
            if (image != null)
            {
                var imagePath = Path.Combine(asmDir, image.ImagePath);
                _context.ProductImages.Remove(image);
                _context.SaveChanges();
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath); 
                }
            }
        }
        public IList<ProductImageDTO> GetProductImages(int productId)
        {
            List<ProductImage> productImages = _context.ProductImages.Where(p => p.ProductId == productId).ToList();
            List<ProductImageDTO> productImageDTOs = new();
            if (productImages.Count > 0)
            {
                string assemblyDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
                List<string> relPaths = productImages.Select(p => p.ImagePath).ToList();
                List<string> imagesBase64 = ImageUtilities.GetImagesInBase64(ImageUtilities.GetImagePathsFromAssembly(assemblyDir, relPaths));

                for (int i = 0; i < relPaths.Count; i++)
                {
                    productImageDTOs.Add(new ProductImageDTO
                    {
                        ProductImageId = productImages[i].ProductImageId,
                        Name = relPaths[i],
                        ImageBase64 = imagesBase64[i]
                    });
                }
            }

            return productImageDTOs;
        }
    }
}
