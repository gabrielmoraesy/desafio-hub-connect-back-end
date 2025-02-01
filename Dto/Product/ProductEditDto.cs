using System.ComponentModel.DataAnnotations;

namespace api_products_hub_connect.Dto.Product
{
    public class ProductEditDto
    {
        // Alterando de 'required' para atributos do DataAnnotations para validação, caso necessário
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [RegularExpression("^(Eletrônico|Roupas|Alimentos|Livros|Outros)$", ErrorMessage = "Categoria inválida.")]
        public string Category { get; set; }

        // Alterando para IFormFile para permitir o upload de arquivos de imagem
        public IFormFile? Image { get; set; }
        public int IdProduct { get; set; } // Tornando a propriedade pública para facilitar o acesso
    }
}
