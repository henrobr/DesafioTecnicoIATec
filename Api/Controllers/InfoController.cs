using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DesafioTecnicoContext context;

        public InfoController(IConfiguration configuration, DesafioTecnicoContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpGet]
        public async Task<JsonResult> Index()
        {
            int st = 1;
            string msg = DateTime.Now.ToString("G");
            string cnx = "Fail";

            try
            {
                if (await context.Database.CanConnectAsync())
                    cnx = "Success";
            }
            catch (Exception ex)
            {
                st = 0;
                msg = ex.Message;
            }

            return new JsonResult(new { Status = st, Message = msg, Sistema = configuration["Configs:Sistema"], Versao = configuration["Configs:Versao"], Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), ConexaoDB = cnx });
        }
    }
}
