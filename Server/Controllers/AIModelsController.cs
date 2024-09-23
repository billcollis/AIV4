using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AIV4.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIModelsController : ControllerBase 
    {
        public AIModelsModel Get()
        {
            AIModelsModel aimodels;
            try
            {
                var json = System.IO.File.ReadAllText("Data\\AIModels.json");
                aimodels = JsonConvert.DeserializeObject<AIModelsModel>(json);
            }
            catch (Exception ex)
            {
                var a = ex;
                aimodels = new AIModelsModel();
            }
            return aimodels;
        }
    }
}
