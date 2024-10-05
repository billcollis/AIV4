using System.Collections;

namespace AIV4.Shared
{

    public class PromptsModel 
    {
        public Prompts? prompts { get; set; }

    }

    public class Prompts
    {
        public List<string>? teachme { get; set; }
        public List<string>? unpack { get; set; }
        public List<string>? stepup { get; set; }
        public List<string>? unitbuilder { get; set; }
        public List<Knowledgescope>? knowledgescope { get; set; }
        public List<Chatidentity>? chatidentities { get; set; }
        public List<Problemtype>? problemtypes { get; set; }
        public List<Codehelperprompt>? codehelperprompts { get; set; }
    }

    public class Chatidentity
    {
        public string? chatidentity { get; set; }
        public string? prompt { get; set; }
    }

    public class Codehelperprompt
    {
        public string? keyword { get; set; }
        public string? prompt { get; set; }

    }

    public class CodeHelperPrograms
    {
        public List<Codehelperprogram> codehelperprograms = new ();  
    }

    public class Codehelperprogram
    {
        public string? name { get; set; }
        public string? language { get; set; }
        public string? code { get; set; }
    }


    public class Knowledgescope
    {
        public string? name { get; set; }
        public string? scope { get; set; }

    }
    public class Problemtype
    {
        public string? type { get; set; }
        public string? prompt { get; set; }

    }


}












