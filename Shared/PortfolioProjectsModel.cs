using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIV4.Shared
{
    public class PortfolioProjectsModel
    {
        public List<PortfolioProject> pprojects { get; set; }
    }

    public class PortfolioProject
    {
        public string? project_name { get; set; }
        public string? image_src { get; set; }
        public string? image_alt { get; set; }
        public string? project_description { get; set; }
        public string? project_url { get; set; }
        public List<string>? tech_list { get; set; }
    }
}
