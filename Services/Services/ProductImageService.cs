using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Contracts;
using Utilities;
using WebAPI.DTO;

namespace ToysAndGames.Services.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly ToysAndGamesContext _context;
        public ProductImageService(ToysAndGamesContext context)
        {
            _context = context;
        }

        public async Task AddProductImage(IList<ProductImageDTO> productImages, int productId)
        {
            if (productImages.Count > 0 || productImages is not null)
            {
                try
                {
                    string? assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (assemblyDirectory is not null)
                    {
                        List<ProductImage> images = new();
                        //TODO: What will happen if product Images is 0, or null ?
                        foreach (var image in productImages)
                        {
                            //TODO: Same, catch exceptions, validate a null directory
                            string fileName = ImageUtilities.GenerateImageName();
                            //TODO: use string interpolation
                            //TODO: Change the names to something more understandable
                            string relativePath = $"Images\\{fileName}.jpg";

                            images.Add(new() { ProductId = productId, ImagePath = relativePath });
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
                        await _context.ProductImages.AddRangeAsync(images);
                        await _context.SaveChangesAsync(); 
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task DeleteProductImage(int id)
        {
            try
            {
                string? assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var image = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);
                if (image is not null && assemblyDirectory is not null)
                {
                    var imagePath = Path.Combine(assemblyDirectory, image.ImagePath);
                    _context.ProductImages.Remove(image);
                    await _context.SaveChangesAsync();
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IList<ProductImageDTO>> GetProductImages(int productId)
        {
            List<ProductImageDTO> productImageDTOs = new();
            try
            {
                var productImages = await _context.ProductImages.Where(p => p.ProductId == productId).ToListAsync();
                string? assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (productImages.Count > 0 && assemblyDirectory is not null)
                {
                    List<string> relativePaths = productImages.Select(p => p.ImagePath).ToList();
                    List<string> imagesBase64 = ImageUtilities.GetImagesInBase64(ImageUtilities.GetImagePathsFromAssembly(assemblyDirectory, relativePaths));
                    if (imagesBase64.Count>0)
                    {

                        for (int i = 0; i < relativePaths.Count; i++)
                        {
                            productImageDTOs.Add(new ProductImageDTO
                            {
                                Id = productImages[i].Id,
                                Name = relativePaths[i],
                                ImageBase64 = imagesBase64[i]
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return productImageDTOs;
            }

            return productImageDTOs;
        }

        public async Task<bool> ProductImageExists(int id)
        {
            try
            {
                var productImage = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);
                return productImage is not null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
