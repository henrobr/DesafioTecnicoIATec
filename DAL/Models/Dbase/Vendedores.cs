
namespace DAL.Models.Dbase
{
    public class Vendedores
    {
        public Vendedores()
        {
            Vendas = new HashSet<Vendas>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nome do Vendedor")]
        [Required(ErrorMessage = "Nome deve ser informado", AllowEmptyStrings = false)]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 carateres")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nome { get; set; }
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Cpf deve ser informado", AllowEmptyStrings = false)]
        [MinLength(11, ErrorMessage = "Nome deve ter ao menos 11 caracteres")]
        [MaxLength(11, ErrorMessage = "Nome deve ter no máximo 11 carateres")]
        [Column(TypeName = "nvarchar(11)")]
        public string Cpf { get; set; }

        //Vendas
        [InverseProperty(nameof(Dbase.Vendas.IdVendedorNavigation))]
        public ICollection<Vendas> Vendas { get; set; }
    }
}
