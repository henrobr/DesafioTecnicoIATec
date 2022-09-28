namespace DAL.Models.Forms
{
    public class ClientesForm
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome deve ser informado")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 carateres")]
        public string Nome { get; set; }
    }
}
