using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DesafioTecnicoContext context;
        private int st = 1;
        private string msg = "Dados gravados com sucesso";
        private DateTime dtHoje = DateTime.Now.AddSeconds(-20);

        public ClientesController(IConfiguration configuration, DesafioTecnicoContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost]
        [Route("New")]
        public async Task<JsonResult> New(ClientesForm form)
        {
            List<ValidationResult> erros = new List<ValidationResult>();

            int IdCliente = 0;

            try
            {
                //verifica o form enviado
                erros = new ModelValidation().Result(form);

                //se houver erro cria a exception
                if (erros.Count > 0)
                    throw new Exception("er");

                //tudo certo monta o model
                Clientes cliente = new Clientes();
                cliente.Nome = form.Nome.ToUpper();

                //adiciona no context e salva as informacoes
                await context.Clientes.AddAsync(cliente);
                await context.SaveChangesAsync();

                IdCliente = cliente.Id;
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

            return new JsonResult(new { Status = st, Message = msg, IdCliente });
        }
        [HttpGet]
        [Route("List")]
        public async Task<JsonResult> List()
        {
            try
            {
                //Retorna os clientes
                var clientes = await context.Clientes.Select(s => new { Id = s.Id, Nome = s.Nome }).ToListAsync();

                if (clientes.Count == 0)
                    throw new Exception("Não há clientes cadastrados");

                return new JsonResult(new { Status = st, Message = "Dados retornado com sucesso", clientes });
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
