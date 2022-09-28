
namespace DAL.Models.Dbase
{
    public class Produtos
    {
        public Produtos()
        {
            VendasItens = new HashSet<VendasItens>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nome do Produto")]
        [Required(ErrorMessage = "Nome deve ser informado")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 carateres")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nome { get; set; }
        [Display(Name = "Valor de Venda")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorVenda { get; set; } = (decimal)0.00;
        [Display(Name = "Valor de Compra")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorCompra { get; set; } = (decimal)0.00;

        //Itens de vendas
        [InverseProperty(nameof(Dbase.VendasItens.IdProdutoNavigation))]
        public ICollection<VendasItens> VendasItens { get; set; }
    }
}
