using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AITooling.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromptsController : ControllerBase
    {
        [HttpGet]
        public PromptsModel Get()
        {
            PromptsModel allprompts;
            try
            {
                var json = System.IO.File.ReadAllText("Data\\Prompts.json");
                allprompts = JsonConvert.DeserializeObject<PromptsModel>(json);
                return allprompts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                allprompts = new PromptsModel();
                return new PromptsModel();               
            }
            
        }
    }
}
