using System.ComponentModel.DataAnnotations;

namespace api_products_hub_connect.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres.")]
        public required string Name { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [RegularExpression("^(Eletrônico|Roupas|Alimentos|Livros|Outros)$",
            ErrorMessage = "A categoria deve ser uma das opções válidas: Eletrônico, Roupas, Alimentos, Livros ou Outros.")]
        public required string Category { get; set; }

        [DataType(DataType.Upload)]
        [MaxLength(2097152, ErrorMessage = "O tamanho da imagem não pode exceder 2 MB.")]
        [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "A imagem deve ser nos formatos: JPG, JPEG ou PNG.")]
        public string? ImagePath { get; set; } 
    }
}
