using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIV4.Shared
{
    public class AIModelsModel
    {
        public List<string>? aimodels { get; set; }
        
        public List<string> getAIModels()
        {
            if ( aimodels==null)
            {
                aimodels = new List<string>();
            }
            return aimodels; 
        }

    }

}
