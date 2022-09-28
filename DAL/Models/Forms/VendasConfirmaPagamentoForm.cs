namespace DAL.Models.Forms
{
    public class VendasConfirmaPagamentoForm
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID da Venda inválido")]
        public int IdVenda { get; set; } = 0;
        [Required(ErrorMessage = "Data de Pagamento inválida")]
        [DataType(DataType.DateTime)]
        public DateTime DtPagamento { get; set; } = DateTime.Now;
        public decimal Valor { get; set; } = (decimal)0.00;
    }
}
