using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class VendedoresController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DesafioTecnicoContext context;
        private int st = 1;
        private string msg = "Dados gravados com sucesso";
        private DateTime dtHoje = DateTime.Now.AddSeconds(-20);

        public VendedoresController(IConfiguration configuration, DesafioTecnicoContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost]
        [Route("New")]
        public async Task<JsonResult> New(VendedoresForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            int IdVendedor = 0;

            try
            {
                form.Cpf = form.Cpf.Replace(".", "").Replace("-", "");
                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //tudo certo monta o model
                Vendedores vendedor = new Vendedores();
                vendedor.Nome = form.Nome.ToUpper();
                vendedor.Cpf = form.Cpf;

                //adiciona no context e salva as informacoes
                await context.Vendedores.AddAsync(vendedor);
                await context.SaveChangesAsync();

                IdVendedor = vendedor.Id;
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

            return new JsonResult(new { Status = st, Message = msg, IdVendedor });
        }
        [HttpGet]
        [Route("List")]
        public async Task<JsonResult> List()
        {
            try
            {
                //Retorna os vendedores
                var vendedores = await context.Vendedores.Select(s => new { Id = s.Id, Nome = s.Nome, Cpf = s.Cpf }).ToListAsync();

                if (vendedores.Count == 0)
                    throw new Exception("Não há vendedores cadastrados");

                return new JsonResult(new { Status = st, Message = "Dados retornado com sucesso", vendedores });
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
