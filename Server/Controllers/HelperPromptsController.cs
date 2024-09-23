using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AIV4.Server.Controllers
{
    public class HelperPromptsController : ControllerBase
    {
        [HttpGet]
        public Dictionary<string,string> Get()
        {
            PromptsModel allprompts;
            try
            {
                //var json = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"data\\AUCurrV9a.json"}");
                var json = System.IO.File.ReadAllText("Data\\Prompts.json");
                allprompts = JsonConvert.DeserializeObject<PromptsModel>(json);
                if (allprompts == null)
                {
                    return new Dictionary<string, string>();
                }
                //we want key words and prompts
                Dictionary<string,string> result = new Dictionary<string, string>();
                foreach (var prompt in allprompts.prompts.codehelperprompts)
                {
                    result.Add(prompt.keyword, prompt.prompt);
                }
                return result;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                return new Dictionary<string, string>();
            }

        }
    }
}
