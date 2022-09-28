
namespace DAL.Models.Dbase
{
    public class TbStatus
    {
        public TbStatus()
        {
            Vendas = new HashSet<Vendas>();
            VendasHistoricos = new HashSet<VendasHistoricos>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nome do Status")]
        [Required(ErrorMessage = "Nome deve ser informado")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 carateres")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nome { get; set; }

        //Vendas
        [InverseProperty(nameof(Dbase.Vendas.IdStatusNavigation))]
        public ICollection<Vendas> Vendas { get; set; }

        //Historicos de vendas
        [InverseProperty(nameof(Dbase.VendasHistoricos.IdStatusNavigation))]
        public ICollection<VendasHistoricos> VendasHistoricos { get; set; }
    }
}
