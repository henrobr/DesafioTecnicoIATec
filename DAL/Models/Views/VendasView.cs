namespace DAL.Models.Views
{
    public class VendasView
    {
        public int IdVenda { get; set; }
        public string DtVenda { get; set; }
        public string Status { get; set; }
        public int IdVendedor { get; set; }
        public string NomeVendedor { get; set; }
        public string CpfVendedor { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public List<Itens> ItensVenda { get; set; }
        public decimal ValorTotal { get; set; }
    }
    public class Itens
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public int Qtde { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }
}
