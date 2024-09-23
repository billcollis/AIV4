using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIV4.Shared
{
    public class BBPromptsModel
    {
        public List<BBPrompt> bbprompts { get; set; }

        public string getPrompt(String promptName)
        {
            var prompt = bbprompts
                .Where(x => x.bbtype == promptName).ToArray()[0].bbprompt;

            if (prompt != null)
            {
                return prompt;
            }
            return "";        
        }
    }
    public class BBPrompt
    {
        public string? bbtype { get; set; }
        public string? bbdescription { get; set; }
        public string? bbexplanation { get; set; }
        public string? bbimage {  get; set; }
        public string? bbprompt { get; set; }

    }
}


