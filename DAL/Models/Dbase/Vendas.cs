
namespace DAL.Models.Dbase
{
    public class Vendas
    {
        public Vendas()
        {
            Itens = new HashSet<VendasItens>();
            Historicos = new HashSet<VendasHistoricos>();
        }
        [Key]
        public int Id { get; set; }
        public int IdStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DtVenda { get; set; } = DateTime.Now;
        public int IdVendedor { get; set; }
        public int IdCliente { get; set; }

        //Status
        [ForeignKey(nameof(IdStatus))]
        public TbStatus IdStatusNavigation { get; set; }

        //Vendedor
        [ForeignKey(nameof(IdVendedor))]
        public Vendedores IdVendedorNavigation { get; set; }

        //Cliente
        [ForeignKey(nameof(IdCliente))]
        public Clientes IdClienteNavigation { get; set; }

        //Itens de Vendas
        [InverseProperty(nameof(VendasItens.IdVendaNavigation))]
        public ICollection<VendasItens> Itens { get; set; }

        //Historicos
        [InverseProperty(nameof(VendasHistoricos.IdVendaNavigation))]
        public ICollection<VendasHistoricos> Historicos { get; set; }
    }
}
