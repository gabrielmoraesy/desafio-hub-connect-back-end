using api_products_hub_connect.Dto.Product;
using api_products_hub_connect.Models;

namespace api_products_hub_connect.Services.Product
{
    public interface IProductInterface
    {
        Task<ResponseModel<List<ProductModel>>> GetProducts();
        Task<ResponseModel<ProductModel>> GetProductById(int idProduct);
        Task<ResponseModel<ProductModel>> CreateProduct(ProductCreateDto productCreateDto);
        Task<ResponseModel<List<ProductModel>>> EditProduct(int idProduct, ProductEditDto productEditDto);
        Task<ResponseModel<List<ProductModel>>> RemoveProduct(int idProduct);
    }
}
