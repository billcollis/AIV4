using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AIV4.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BBPromptsController : ControllerBase
    {
        public BBPromptsModel Get()
        {
            BBPromptsModel bbprompts;
            try
            {                
                var json = System.IO.File.ReadAllText("Data\\BBPrompts.json");
                bbprompts = JsonConvert.DeserializeObject<BBPromptsModel>(json);
            }
            catch (Exception ex) {
                var a = ex;
                bbprompts = new BBPromptsModel();
            }
            return bbprompts;
        }
    }
}
