using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DesafioTecnicoContext context;
        private int st = 1;
        private string msg = "Dados gravados com sucesso";
        private DateTime dtHoje = DateTime.Now.AddSeconds(-20);

        public ProdutosController(IConfiguration configuration, DesafioTecnicoContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost]
        [Route("New")]
        public async Task<JsonResult> New(ProdutosForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            int IdProduto = 0;

            try
            {
                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //tudo certo monta o model
                Produtos produto = new Produtos();
                produto.Nome = form.Nome.ToUpper();
                produto.ValorCompra = form.ValorCompra;
                produto.ValorVenda = form.ValorVenda;

                //adiciona no context e salva as informacoes
                await context.Produtos.AddAsync(produto);
                await context.SaveChangesAsync();

                IdProduto = produto.Id;
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

            return new JsonResult(new { Status = st, Message = msg, IdProduto });
        }
        [HttpGet]
        [Route("List")]
        public async Task<JsonResult> List()
        {
            try
            {
                //Retorna os produtos
                var produtos = await context.Produtos.Select(s => new { Id = s.Id, Nome = s.Nome, ValorVenda = s.ValorVenda, ValorCompra = s.ValorCompra }).ToListAsync();

                if (produtos.Count == 0)
                    throw new Exception("Não há produtos cadastrados");

                return new JsonResult(new { Status = st, Message = "Dados retornado com sucesso", produtos });
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
    }
}
