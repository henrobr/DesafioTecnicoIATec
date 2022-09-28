namespace DAL.Models.Forms
{
    public class VendasItensForm
    {
        public int IdProduto { get; set; }
        [Display(Name = "Qtde")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade inválida")]
        public int Qtde { get; set; } = 1;
        //[Display(Name = "Valor Unitário")]
        //[Range((double)0.01, (double)decimal.MaxValue, ErrorMessage = "Valor Unitário deve ser igual ou maior que 0,01")]
        //public decimal ValorUnitario { get; set; }
    }
}
