
namespace DAL.Models.Dbase
{
    public class VendasItens
    {
        [Key]
        public int Id { get; set; }
        public int IdVenda { get; set; }
        public int IdProduto { get; set; }
        [Display(Name = "Qtde")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade inválida")]
        public int Qtde { get; set; } = 1;
        [Display(Name = "Valor Unitário")]
        [Range((double)0.01, (double)decimal.MaxValue, ErrorMessage = "Valor Unitário deve ser igual ou maior que 0,01")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorUnitario { get; set; }

        //Venda
        [ForeignKey(nameof(IdVenda))]
        public Vendas IdVendaNavigation { get; set; }

        //Produto
        [ForeignKey(nameof(IdProduto))]
        public Produtos IdProdutoNavigation { get; set; }

    }
}
