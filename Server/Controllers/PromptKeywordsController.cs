using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AIV4.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromptKeywordsController : ControllerBase
    {
        [HttpGet]
        public List<Codehelperprompt> Get()
        {
            PromptsModel allprompts;
            try
            {
                

                //var json = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"data\\AUCurrV9a.json"}");
                var json = System.IO.File.ReadAllText("Data\\Prompts.json");
                allprompts = JsonConvert.DeserializeObject<PromptsModel>(json);
                if (allprompts == null)
                {
                    return new List<Codehelperprompt>();
                }
                //we only want the key words to be returned not the prompts???
                var result = new List<Codehelperprompt>();
                foreach (var prompt in allprompts.prompts.codehelperprompts) //change this to return the full prompts and then show them in UI
                {
                    var keywordprompt = new Codehelperprompt();
                    keywordprompt.prompt = prompt.prompt;
                    keywordprompt.keyword = prompt.keyword; 
                    result.Add(keywordprompt);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new List<Codehelperprompt>();
            }

        }

    }
}
