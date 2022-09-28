
namespace DAL.Models.Dbase
{
    public class Clientes
    {
        public Clientes()
        {
            Vendas = new HashSet<Vendas>();
        }
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Nome { get; set; }

        //Vendas
        [InverseProperty(nameof(Dbase.Vendas.IdClienteNavigation))]
        public ICollection<Vendas> Vendas { get; set; }
    }
}
