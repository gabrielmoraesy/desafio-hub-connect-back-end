using api_products_hub_connect.Dto.Product;
using api_products_hub_connect.Models;
using api_products_hub_connect.Services.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace api_products_hub_connect.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface _productInterface;

        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        public ProductController(IProductInterface productInterface)
        {
            _productInterface = productInterface;

            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetProducts()
        {
            var result = await _productInterface.GetProducts();
            if (!result.Status)
                return StatusCode(500, result.Mensagem);

            return Ok(result);
        }

        [HttpGet("{idProduct}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> GetProductById(int idProduct)
        {
            var result = await _productInterface.GetProductById(idProduct);
            if (!result.Status)
                return StatusCode(404, result.Mensagem);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productCreateDto)
        {
            string imagePath = null;

            try
            {
                if (productCreateDto.Image != null)
                {
                    if (productCreateDto.Image.Length > 2 * 1024 * 1024) // 2MB
                    {
                        return BadRequest("O tamanho da imagem não pode exceder 2MB.");
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(productCreateDto.Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest("Formato de imagem inválido. Apenas JPG, JPEG ou PNG são permitidos.");
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    imagePath = Path.Combine(_uploadFolderPath, uniqueFileName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await productCreateDto.Image.CopyToAsync(fileStream);
                    }

                    productCreateDto.ImagePath = $"Uploads/{uniqueFileName}";
                }

                var result = await _productInterface.CreateProduct(productCreateDto);

                if (!result.Status)
                    return StatusCode(500, result.Mensagem);

                return Created("/Product", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        [HttpPatch("{idProduct}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> EditProduct(int idProduct, [FromForm] ProductEditDto productEditDto)
        {
            if (productEditDto.Image != null)
            {
                if (productEditDto.Image.Length > 2 * 1024 * 1024) // 2MB
                {
                    return BadRequest("O tamanho da imagem não pode exceder 2MB.");
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(productEditDto.Image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest("Formato de imagem inválido. Apenas JPG, JPEG ou PNG são permitidos.");
                }

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await productEditDto.Image.CopyToAsync(fileStream);
                }
            }

            var result = await _productInterface.EditProduct(idProduct, productEditDto);

            if (!result.Status)
                return StatusCode(500, result.Mensagem);

            return Ok(result);
        }

        [HttpDelete("{idProduct}")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> RemoveProduct(int idProduct)
        {
            var result = await _productInterface.RemoveProduct(idProduct);
            if (!result.Status)
                return StatusCode(500, result.Mensagem);

            return Ok(result);
        }
    }
}
