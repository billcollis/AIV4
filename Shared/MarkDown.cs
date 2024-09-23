using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Markdig;


namespace AIV4.Shared
{

    /// <summary>
    ///
    /// Parse a full line of tokens from markdown into Html or code
    /// 
    /// start of multiline code block
    ///    ```languagename \n  e.g. ```python\n
    /// end of multiline codeblock
    ///     ```\n
    /// single line of code
    ///     ```langname print("hello world"); ```
    ///  => <pre><code class="language-langname">print("hello world");</code></pre>
    /// </summary>
    public static class MarkDown
    {

        private static string returnString = "";
        private static State state = State.IsText;
        private static string language = "";
        public static string Parse(string inputText)
        {
            int codeMakersCount = Regex.Matches(inputText, "```").Count;
            if(codeMakersCount == 2) //i cant imagine this will ever happen
            {
                state = State.IsOneLineOfCode;
            }
            if (codeMakersCount == 1) { //could be start or end of multiline code block
                if (inputText.IndexOf("```\n")>-1)
                    state = State.IsCodeEnd;
                else
                    state = State.IsCodeStart;
            }


            switch (state){
                case State.IsText:
                    returnString =  Markdown.ToHtml(inputText);
                    break;
                case State.IsCodeStart:
                    // format and get language
                    language = inputText.Replace("```", "").Replace("\n", "");
                    //if (language=="html") 
                        //language="text";
                    returnString = $"<pre><code class='language-{language}'\n>";
                    state = State.IsLineWithinCode;
                    break;
                case State.IsLineWithinCode: //just return the line of code without marking it up
                    //escape  html  < with &lt;

                    returnString = inputText.Replace("<","&lt;");
                    break;
                case State.IsCodeEnd:
                    returnString = "</code></pre>\n";
                    state = State.IsText;
                    break;
                case State.IsOneLineOfCode:
                    break;
            }
            return returnString;    

                
            //returnString = Markdown.ToHtml(unMarkedLine);
            //if (!isCode && returnString.IndexOf("<pre><code class=") > -1)
            //{
            //    returnString = returnString.Replace("</code></pre>", "");//remove closing tags
            //}
            //var open = returnString.IndexOf("<pre><code>");
            //var close = returnString.IndexOf("</code></pre>");
            //if (returnString.IndexOf("<pre><code>")==0 && returnString.IndexOf("/code></pre>")<14)
            //{
            //    returnString = returnString.Replace("<code><pre>", "");//remove closing tags
            //}
            //unMarkedLine = "";
        }
        private enum State
        {
            IsText,
            IsOneLineOfCode, //has both ``` and ``` in one line
            IsCodeStart,        //has ```languagename but not closing ```
            IsLineWithinCode,   
            IsCodeEnd,
            LineComplete
        }
    }
}
