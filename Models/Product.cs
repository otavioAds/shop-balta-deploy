
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MaxLength(60, ErrorMessage = "Este campo deve ser entre 3 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Este campo deve ser entre 3 60 caracteres!")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter no máximo 1024 caracteres!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "o preço deve ser maior que zero!")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Categoria Inválida!")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}