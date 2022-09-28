namespace DAL.Models.Forms
{
    public class VendasForm
    {
        public int IdVendedor { get; set; }
        public int IdCliente { get; set; }
        public List<VendasItensForm> Itens { get; set; }
    }
}
