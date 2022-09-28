
namespace DAL.Models.Dbase
{
    public class VendasHistoricos
    {
        [Key]
        public int Id { get; set; }
        public int IdVenda { get; set; }
        public int IdStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DtInsert { get; set; } = DateTime.Now;
        public string Historico { get; set; }

        //Vendas
        [ForeignKey(nameof(IdVenda))]
        public Vendas IdVendaNavigation { get; set; }

        //Status 
        [ForeignKey(nameof(IdStatus))]
        public TbStatus IdStatusNavigation { get; set; }
    }
}
