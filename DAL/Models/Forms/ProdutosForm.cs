
namespace DAL.Models.Dbase
{
    public class ProdutosForm
    {
        [Display(Name = "Nome do Produto")]
        [Required(ErrorMessage = "Nome deve ser informado")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 carateres")]
        public string Nome { get; set; }
        [Range((double)0.01, (double)decimal.MaxValue, ErrorMessage = "Valor Unitário deve ser igual ou maior que 0,01")]
        [Display(Name = "Valor de Venda")]
        public decimal ValorVenda { get; set; } = (decimal)0.01;
        [Range((double)0.01, (double)decimal.MaxValue, ErrorMessage = "Valor Unitário deve ser igual ou maior que 0,01")]
        [Display(Name = "Valor de Compra")]
        public decimal ValorCompra { get; set; } = (decimal)0.01;
    }
}
