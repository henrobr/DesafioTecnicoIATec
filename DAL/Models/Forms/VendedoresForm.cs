namespace DAL.Models.Forms
{
    public class VendedoresForm
    {
        [Display(Name = "Nome do Vendedor")]
        [Required(ErrorMessage = "Nome deve ser informado")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 carateres")]
        public string Nome { get; set; }
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Cpf deve ser informado", AllowEmptyStrings = false)]
        [MinLength(11, ErrorMessage = "Nome deve ter ao menos 11 caracteres")]
        [MaxLength(14, ErrorMessage = "Nome deve ter no máximo 11 carateres")]
        public string Cpf { get; set; }
    }
}
