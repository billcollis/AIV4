using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AIV4.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeHelperProgramsController : ControllerBase
    {
        [HttpGet]
        public Codehelperprogram Get(string programName)
        {
            //var url = "https://drive.google.com/file/d/1WH08PK8KIqyeXm76Xu4tl4FS6eFNIu78/view?usp=sharing";
            //var textfromfile = new HttpClient().GetStringAsync(url).Result;

            var json = System.IO.File.ReadAllText("Data\\CodeHelperPrograms.json");
            var allprograms = JsonConvert.DeserializeObject<CodeHelperPrograms>(json);
            var program = new Codehelperprogram();
            foreach (var pr in allprograms!.codehelperprograms)
            {
                if (pr.name == programName)
                {
                    return pr;
                }
            }
            return new Codehelperprogram();
        }
    }
}
