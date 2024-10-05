using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using static System.Net.WebRequestMethods;

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
                var url = "https://drive.google.com/file/d/1WH08PK8KIqyeXm76Xu4tl4FS6eFNIu78/view?usp=sharing";
                var textfromfile = new HttpClient().GetStringAsync(url); 

                var json = System.IO.File.ReadAllText("Data\\Prompts.json");  // https://drive.google.com/file/d/1WH08PK8KIqyeXm76Xu4tl4FS6eFNIu78/view?usp=sharing
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
