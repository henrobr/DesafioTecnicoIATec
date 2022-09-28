using DAL.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DesafioTecnicoContext context;
        private int st = 1;
        private string msg = "Dados gravados com sucesso";
        private DateTime dtHoje = DateTime.Now.AddSeconds(-20);

        public VendasController(IConfiguration configuration, DesafioTecnicoContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost]
        [Route("New")]
        public async Task<JsonResult> New(VendasForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            int IdVenda = 0;

            try
            {
                //model
                Vendas venda = new Vendas();

                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //Verifica cliente
                var cliente = await context.Clientes.SingleOrDefaultAsync(f => f.Id == form.IdCliente);
                if (cliente == null)
                    erros.Add(new ValidationResult("Cliente não encontrado. "));

                //verifica vendedor
                var vendedor = await context.Vendedores.SingleOrDefaultAsync(f => f.Id == form.IdVendedor);
                if (vendedor == null)
                    erros.Add(new ValidationResult("Vendedor não encontrado. "));

                if (form.Itens.Count == 0)
                    erros.Add(new ValidationResult("Ao menos 1 item deve ter na venda. "));
                else
                    foreach (var i in form.Itens)
                    {
                        var produto = await context.Produtos.AsNoTracking().SingleOrDefaultAsync(f => f.Id == i.IdProduto);
                        if (produto == null)
                            erros.Add(new ValidationResult("Produto não encontrado, ID: " + i.IdProduto + "."));
                        else
                            //Adiciona os itens 
                            venda.Itens.Add(new VendasItens() { IdProduto = i.IdProduto, Qtde = i.Qtde, ValorUnitario = produto.ValorVenda });
                    }

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //Conclui o model
                venda.IdCliente = form.IdCliente;
                venda.IdVendedor = form.IdVendedor;
                venda.DtVenda = dtHoje;
                venda.IdStatus = 2;

                //cria os historicos da venda
                //Venda Realizada
                venda.Historicos.Add(new VendasHistoricos() { IdStatus = 1, DtInsert = dtHoje, Historico = "Venda Realizada" });
                //Aguardando Pagamento
                venda.Historicos.Add(new VendasHistoricos() { IdStatus = 2, DtInsert = dtHoje.AddSeconds(20), Historico = "Aguardando Pagamento" });

                //adiciona no context e salva as informacoes
                await context.Vendas.AddAsync(venda);
                await context.SaveChangesAsync();

                IdVenda = venda.Id;
            }
            catch (Exception ex)
            {
                st = 0;
                msg = "";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    msg = excp + " ";
                if (erros.Count > 0)
                {
                    foreach (var e in erros)
                    {
                        msg += e.ErrorMessage + " ";
                    }
                }
            }

            return new JsonResult(new { Status = st, Message = msg, IdVenda });
        }
        [HttpGet]
        [Route("View/{idvenda}")]
        public async Task<JsonResult> Details(int idvenda)
        {
            try
            {
                //buscando a venda com  o id
                var ctx = await context.Vendas
                                                .Include(i => i.IdStatusNavigation)
                                                .Include(i => i.IdClienteNavigation)
                                                .Include(i => i.IdVendedorNavigation)
                                                .Include(i => i.Itens).ThenInclude(i => i.IdProdutoNavigation)
                                                //.Select(s => new
                                                //{
                                                //    Id = s.Id,
                                                //    Status = s.IdStatusNavigation.Nome,
                                                //    DtVenda = s.DtVenda,
                                                //    IdVendedor = s.IdVendedor,
                                                //    NomeVendedor = s.IdVendedorNavigation.Nome,
                                                //    CpfVendedor = s.IdVendedorNavigation.Cpf,
                                                //    IdCliente = s.IdCliente,
                                                //    NomeCliente = s.IdClienteNavigation.Nome,
                                                //    ItensDaVenda = s.Itens.Select(si => new
                                                //    {
                                                //        IdProduto = si.IdProduto,
                                                //        NomeProduto = si.IdProdutoNavigation.Nome,
                                                //        Qtde = si.Qtde,
                                                //        ValorUnitario = si.ValorUnitario,
                                                //        SubTotal = (decimal)(si.Qtde * si.ValorUnitario)
                                                //    }),
                                                //    ValorTotal = (decimal)0.00
                                                //})
                                                .AsNoTracking()
                                                .Where(w => w.Id == idvenda)
                                                .SingleOrDefaultAsync();

                //verifica se a venda existe
                if (ctx == null)
                    throw new Exception("Venda não encontrada");

                var venda = new VendasView();
                venda.IdVenda = ctx.Id;
                venda.Status = ctx.IdStatusNavigation.Nome;
                venda.DtVenda = ctx.DtVenda.ToString("G");
                venda.IdVendedor = ctx.IdVendedor;
                venda.NomeVendedor = ctx.IdVendedorNavigation.Nome;
                venda.CpfVendedor = ctx.IdVendedorNavigation.Cpf;
                venda.IdCliente = ctx.IdCliente;
                venda.NomeCliente = ctx.IdClienteNavigation.Nome;
                venda.ItensVenda = ctx.Itens.Select(s => new Itens() { IdProduto = s.IdProduto, NomeProduto = s.IdProdutoNavigation.Nome, Qtde = s.Qtde, ValorUnitario = s.ValorUnitario, SubTotal = (s.Qtde * s.ValorUnitario) }).ToList();
                venda.ValorTotal = venda.ItensVenda.Sum(s => s.SubTotal);


                return new JsonResult(new { Status = st, Message = "Dados retornados com sucesso", venda });
            }
            catch (Exception ex)
            {
                st = 0;
                msg = "";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    msg = excp + " ";
            }

            return new JsonResult(new { Status = st, Message = msg });
        }
        [HttpPut]
        [Route("ConfirmaPagamento")]
        public async Task<JsonResult> ConfirmaPagamento(VendasConfirmaPagamentoForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            try
            {
                //Data de hoje
                DateTime dtHoje = DateTime.Now.AddSeconds(-20);

                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //busca a venda
                var venda = await context.Vendas
                                                .Include(i => i.Itens)
                                                .Where(w => w.Id == form.IdVenda)
                                                .SingleOrDefaultAsync();

                //verifica se a venda existe
                if (venda == null)
                    throw new Exception("Venda não encontrada");

                //Verificacoes de Status
                if (venda.IdStatus == 6)
                    throw new Exception("Venda já foi entregue.");
                if (venda.IdStatus == 5)
                    throw new Exception("Venda já foi enviada.");
                if (venda.IdStatus == 4)
                    throw new Exception("Venda está cancelada.");
                if (venda.IdStatus == 3)
                    throw new Exception("Venda já está com pagamento aprovado.");

                var itens = venda.Itens.Select(si => new
                {
                    IdProduto = si.IdProduto,
                    Qtde = si.Qtde,
                    ValorUnitario = si.ValorUnitario,
                    SubTotal = (decimal)(si.Qtde * si.ValorUnitario)
                }).ToList();

                var total = itens.Sum(s => s.SubTotal);

                if (form.Valor == total)
                {
                    //Altera o status
                    venda.IdStatus = 3;
                    //Adiciona o Historico do pagamento
                    venda.Historicos.Add(new VendasHistoricos() { IdVenda = venda.Id, DtInsert = dtHoje, IdStatus = venda.IdStatus, Historico = "Pagamento Aprovado. Valor Recebido: " + form.Valor.ToString("C2") });

                    //Atualiza o context e grava as informacoes
                    context.Vendas.Update(venda);
                    await context.SaveChangesAsync();
                }
                else
                    throw new Exception("Valor informado não é igual o valor total da venda");
            }
            catch (Exception ex)
            {
                st = 0;
                msg = "";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    msg = excp + " ";
                if (erros.Count > 0)
                {
                    foreach (var e in erros)
                    {
                        msg += e.ErrorMessage + " ";
                    }
                }
            }

            return new JsonResult(new { Status = st, Message = msg });
        }
        [HttpPut]
        [Route("Envio")]
        public async Task<JsonResult> Envio(VendasEnvioForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            try
            {
                //Data de hoje


                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //busca a venda
                var venda = await context.Vendas
                                                .Include(i => i.Itens)
                                                .Where(w => w.Id == form.IdVenda)
                                                .SingleOrDefaultAsync();

                //verifica se a venda existe
                if (venda == null)
                    throw new Exception("Venda não encontrada");

                //Verificacoes de Status
                if (venda.IdStatus == 6)
                    throw new Exception("Venda já foi entregue.");
                if (venda.IdStatus == 5)
                    throw new Exception("Venda já foi enviada.");
                if (venda.IdStatus == 4)
                    throw new Exception("Venda está cancelada.");
                if (venda.IdStatus == 2)
                    throw new Exception("Venda não tem pagamento aprovado.");

                //Altera o status
                venda.IdStatus = 5;
                //Adiciona o Historico do pagamento
                venda.Historicos.Add(new VendasHistoricos() { IdVenda = venda.Id, DtInsert = dtHoje, IdStatus = venda.IdStatus, Historico = "Enviado. Data do Envio: " + form.DtEnvioEntrega.ToString("G") });

                //Atualiza o context e grava as informacoes
                context.Vendas.Update(venda);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                st = 0;
                msg = "";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    msg = excp + " ";
                if (erros.Count > 0)
                {
                    foreach (var e in erros)
                    {
                        msg += e.ErrorMessage + " ";
                    }
                }
            }

            return new JsonResult(new { Status = st, Message = msg });
        }
        [HttpPut]
        [Route("Entrega")]
        public async Task<JsonResult> Entrega(VendasEnvioForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            try
            {
                //Data de hoje
                DateTime dtHoje = DateTime.Now;

                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //busca a venda
                var venda = await context.Vendas.Where(w => w.Id == form.IdVenda).SingleOrDefaultAsync();

                //verifica se a venda existe
                if (venda == null)
                    throw new Exception("Venda não encontrada");

                //Verificacoes de Status
                if (venda.IdStatus == 6)
                    throw new Exception("Venda já foi entregue.");
                if (venda.IdStatus == 4)
                    throw new Exception("Venda está cancelada.");

                //Verifica se a venda esta como enviada
                if (venda.IdStatus != 5)
                    throw new Exception("Venda não foi enviada.");

                //Altera o status
                venda.IdStatus = 6;
                //Adiciona o Historico do pagamento
                venda.Historicos.Add(new VendasHistoricos() { IdVenda = venda.Id, DtInsert = dtHoje, IdStatus = venda.IdStatus, Historico = "Entregue. Data de Entrega: " + form.DtEnvioEntrega.ToString("G") });

                //Atualiza o context e grava as informacoes
                context.Vendas.Update(venda);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                st = 0;
                msg = "";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    msg = excp + " ";
                if (erros.Count > 0)
                {
                    foreach (var e in erros)
                    {
                        msg += e.ErrorMessage + " ";
                    }
                }
            }

            return new JsonResult(new { Status = st, Message = msg });
        }
        [HttpPut]
        [Route("Cancelar")]
        public async Task<JsonResult> Cancelar(int idvenda)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            try
            {
                //Data de hoje
                DateTime dtHoje = DateTime.Now;

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //busca a venda
                var venda = await context.Vendas.Where(w => w.Id == idvenda).SingleOrDefaultAsync();

                //verifica se a venda existe
                if (venda == null)
                    throw new Exception("Venda não encontrada");

                //Verificacoes de Status
                if (venda.IdStatus == 6)
                    throw new Exception("Venda já foi entregue.");
                if (venda.IdStatus == 5)
                    throw new Exception("Venda já foi enviada.");
                if (venda.IdStatus == 4)
                    throw new Exception("Venda já está cancelada.");

                //Altera o status
                venda.IdStatus = 4;
                //Adiciona o Historico do pagamento
                venda.Historicos.Add(new VendasHistoricos() { IdVenda = venda.Id, DtInsert = dtHoje, IdStatus = venda.IdStatus, Historico = "Venda cancelada. Data de Cancelamento: " + dtHoje.ToString("G") });

                //Atualiza o context e grava as informacoes
                context.Vendas.Update(venda);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                st = 0;
                msg = "";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    msg = excp + " ";
                if (erros.Count > 0)
                {
                    foreach (var e in erros)
                    {
                        msg += e.ErrorMessage + " ";
                    }
                }
            }

            return new JsonResult(new { Status = st, Message = msg });
        }
        //[HttpGet]
        //[Route("List")]
        //public async Task<JsonResult> List()
        //{
        //    try
        //    {
        //        //Retorna os produtos
        //        var vendas = await context.Vendas.Select(s => new { Id = s.Id, Nome = s.Nome }).ToListAsync();

        //        if (vendas.Count == 0)
        //            throw new Exception("Não há vendas realizadas");

        //        return new JsonResult(new { Status = st, Message = "Dados retornado com sucesso", vendas });
        //    }
        //    catch (Exception ex)
        //    {
        //        st = 0;
        //        string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        //        if (excp != "er")
        //            msg = excp + " ";
        //    }

        //    return new JsonResult(new { Status = st, Message = msg });
        //}
    }
}
