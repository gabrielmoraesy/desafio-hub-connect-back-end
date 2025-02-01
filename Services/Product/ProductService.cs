using api_products_hub_connect.Dto.Product;
using api_products_hub_connect.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_products_hub_connect.Services.Product
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ProductModel>> CreateProduct(ProductCreateDto productCreateDto)
        {
            var response = new ResponseModel<ProductModel>();
            try
            {
                var product = new ProductModel
                {
                    Name = productCreateDto.Name,
                    Description = productCreateDto.Description,
                    Price = productCreateDto.Price,
                    Category = productCreateDto.Category
                };

                if (productCreateDto.Image != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productCreateDto.Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productCreateDto.Image.CopyToAsync(stream);
                    }

                    product.ImagePath = Path.Combine("Uploads", fileName).Replace("\\", "/");
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                response.Dados = product;

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }
        public async Task<ResponseModel<List<ProductModel>>> EditProduct(int idProduct, ProductEditDto productEditDto)
        {
            var response = new ResponseModel<List<ProductModel>>();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == idProduct);
                if (product == null)
                    throw new CustomError("Produto não encontrado!", 404);

                product.Name = productEditDto.Name;
                product.Description = productEditDto.Description;
                product.Price = productEditDto.Price;
                product.Category = productEditDto.Category;

                if (productEditDto.Image != null && productEditDto.Image.Length > 0)
                {
                    var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    if (!Directory.Exists(uploadFolderPath))
                        Directory.CreateDirectory(uploadFolderPath);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productEditDto.Image.FileName);
                    var filePath = Path.Combine(uploadFolderPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productEditDto.Image.CopyToAsync(fileStream);
                    }

                    if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
                    {
                        File.Delete(product.ImagePath);
                    }

                    product.ImagePath = Path.Combine("Uploads", fileName);
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Products.ToListAsync();
                response.Mensagem = "Produto editado com sucesso!";
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }


        public async Task<ResponseModel<ProductModel>> GetProductById(int idProduct)
        {
            var response = new ResponseModel<ProductModel>();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == idProduct);
                if (product == null)
                    throw new CustomError("Produto não encontrado!", 404);

                response.Dados = product;
                response.Mensagem = "Produto encontrado!";
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<ProductModel>>> GetProducts()
        {
            var response = new ResponseModel<List<ProductModel>>();
            try
            {
                var products = await _context.Products.ToListAsync();
                if (products == null || products.Count == 0)
                    throw new CustomError("Nenhum produto encontrado!", 404);

                response.Dados = products;
                response.Mensagem = "Produtos retornados com sucesso!";
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<ProductModel>>> RemoveProduct(int idProduct)
        {
            var response = new ResponseModel<List<ProductModel>>();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == idProduct);
                if (product == null)
                    throw new CustomError("Produto não encontrado!", 404);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Products.ToListAsync();
                response.Mensagem = "Produto removido com sucesso!";
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }
            return response;
        }
    }
}
