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
            //TODO: Same, catch exceptions, validate a null directory
            string fileName = string.Empty;
            string relativePath = string.Empty;
            if (productImages.Count > 0)
            {
                string assemblyDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
                List<ProductImage> images = new();
                //TODO: What will happen if product Images is 0, or null ?
                foreach (var image in productImages)
                {
                    fileName = ImageUtilities.GenerateImageName();
                    //TODO: use string interpolation
                    //TODO: Change the names to something more understandable
                    //path = "Images\\" + fileName+ ".jpg";
                    relativePath = $"Images\\{fileName}.jpg";

                    images.Add(new(){ProductId = productId,ImagePath = relativePath });
                    byte[] bytes = Convert.FromBase64String(image.ImageBase64);
                    relativePath = $"{assemblyDirectory}\\{relativePath}";
                    var parentPath = Directory.GetParent(relativePath).FullName;
                    //TODO: This could be shortened to have the Create directory inside the IF
                    if (!Directory.Exists(parentPath))
                    {
                        Directory.CreateDirectory(parentPath);
                    }
                    File.WriteAllBytes(relativePath, bytes);
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
