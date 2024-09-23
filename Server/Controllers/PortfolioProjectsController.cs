using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;

namespace AIV4.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioProjectsController : ControllerBase
    {
        public PortfolioProjectsModel Get()
        {
            PortfolioProjectsModel pprojects;
            try
            {
                var json = System.IO.File.ReadAllText("Data\\PortfolioProjects.json");
                pprojects = JsonConvert.DeserializeObject<PortfolioProjectsModel>(json);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                pprojects = new PortfolioProjectsModel();
            }
            return pprojects;
        }
    }
}
