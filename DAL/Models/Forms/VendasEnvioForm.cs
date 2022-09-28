namespace DAL.Models.Forms
{
    public class VendasEnvioForm
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID da Venda inválido")]
        public int IdVenda { get; set; } = 0;
        [Required(ErrorMessage = "Data de Pagamento inválida")]
        [DataType(DataType.DateTime)]
        public DateTime DtEnvioEntrega { get; set; } = DateTime.Now;
    }
}
