using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.Fast.Components.FluentUI;

namespace AIV4.Shared
{
    public static class CodeParser
    {
        private static int thisTickCount = 0;
        private static bool isCode = false;
        private static bool got3SequentialBackTicks = false;
        private static int nSequentialBackTicks = 0;
        private static string langString = ""; //get language after start of code block 
        private static List<String> languageList = new List<String>();

        /// <summary>
        /// Parse a line returned from OpenAI to see if it is code - 
        /// currently called from all the pages so 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        public static string Parse(string res="", List<string>? languageList=null)
        {
            //Bill Collis
            //maybe this could be changed now - I haven't seen the 3 backticks come through as seperate streamed chars

            //not a particularly great state machine to identify the start and finish of codeblocks
            //the start of a codeblock is  ``` (3 backticks) langname  then a linefeed 
            //the end of a code block is  ``` then a linefeed 
            //the complication is that the three backticks delimiting a code block may be interrupted by a linefeed due to the streaming of the response from chatGPT 
            //also we only want to count 3 sequential backticks


            thisTickCount = countSequentialBackTicks(res); //gets 0,1,2,3.
            nSequentialBackTicks = getNSequentialBackTicks(thisTickCount, nSequentialBackTicks); //get the number of ` found in a row
            if (nSequentialBackTicks == 3) //if there were ever >3 this will think it is the start/finish of a code block and remove three
            {
                //Console.WriteLine("got3```");
                got3SequentialBackTicks = true; //signal to change state
                nSequentialBackTicks = 0;
            }

            //now capture language name
            if (got3SequentialBackTicks && !isCode)//start of code block - get language - and \n
            {
                //console.WriteLine("GOT START THREE \n"); //after testing delete this 

                if (res == "\n")  //this signals the end of the three backticks and langname
                {
                    isCode = true; //signal that we have the language are going to be receiving code for the codeblock
                    got3SequentialBackTicks = false; //reset until we get this again
                    langString = langString.Replace("`", "").Trim().ToLower(); //remove the backticks
                     if (languageList!.Contains(langString))  //the langstring was found
                    {

                    }
                    else//langstring not found
                    {

                    }
                   
                                       
                    if (langString == "html")
                    {
                        langString = "markup";
                    }
                    return("<pre>"+"\n"+"<code class='language-" + langString + "'>"); //this triggers prism to display code

                }
                else //reading the langname
                {
                    langString += res; //keep adding text until \n
                    return "";
                }
            }
            else if (got3SequentialBackTicks && isCode) //end of code block - get \n
            {
                //await AddMessageToChat("GOT END THREE \n"); //after testing delete this 
                // could receive various things here    `\n\n
                if (res == "```" || res == "``" || res == "`") //dont return these
                {
                }
                else if (res == "\n" || res == "\n\n" || res == "`\n\n" || res == "``\n\n")//sometimes there are \n with the backticks
                {
                    res = res.Replace("`", "");
                    isCode = false;
                    got3SequentialBackTicks = false;
                    langString = "";
                    return("</code>"+"\n"+"</pre>");//trigger prism end of code
                }
                else//what else could we get??
                {
                    isCode = false;
                    got3SequentialBackTicks = false;
                }
            }
            else if (nSequentialBackTicks == 2)
            {
                return "";
            }
            else if (langString == "markup" || langString == "javascript")  
            {
                //needed to get html to display - note https://stackoverflow.com/questions/18391425/markup-not-showing-up-using-prism-js/27495949#27495949
                //this didn't work as it stops displaying when it sees </script>
                res = res.Replace("<", "&lt;"); //no need to replace >
            }
            return res;
        }
        /// <summary>
        /// keeps the tally of sequential backticks from 0 to 3
        /// the input could be '  or '' or '''  so we need static function
        /// </summary>
        /// <param name="thisTickCount"></param>
        /// <param name="nSequentialBackTicks"></param>
        /// <returns>nSequentialBackTicks</returns>
        private static int getNSequentialBackTicks(int thisTickCount, int nSequentialBackTicks)
        {
            //if (thisTickCount > 0)
            //{
            //    var a = nSequentialBackTicks;//just a place to pause for debugging purposes to see what is coming in
            //}
            // If thisTickCount is 0, reset nSequentialBackTicks to 0.
            if (thisTickCount == 0)
            {
                nSequentialBackTicks = 0;//make sure we reset this

            }
            // If thisTickCount is 1 or 2, increment nSequentialBackTicks by thisTickCount.
            else if (thisTickCount == 1 || thisTickCount == 2)
            {
                nSequentialBackTicks += thisTickCount;
            }
            else if (thisTickCount == 3)
            {
                nSequentialBackTicks = 3;
            }
            return nSequentialBackTicks;
        }

        /// <summary>
        /// Identify the number of sequential back ticks in a row
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static int countSequentialBackTicks(string str)
        {
            var count = 0;
            foreach (char c in str)
            {
                if (c == '`') //got 1,2,3
                {
                    count++;
                }
                else //exit because its a char other than `
                {
                    return count;
                }
            }
            return count;
        }


    }
}
