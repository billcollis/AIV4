
using AIV4.Shared;


namespace AIV4.Test
{


    [TestFixture]
    public class CodeItTests
    {
        [Test]
        public void Format_StartOfCodeBlock_1Tick()
        {
            string input = "```\n";
            string output = CodeParser.Parse(input);
            string expectedOutput = "";
            Assert.That(output, Is.EqualTo(expectedOutput));

            input = "javascript\n";
            output = CodeParser.Parse(input);
            expectedOutput = "";
            Assert.That(output, Is.EqualTo(expectedOutput));
            
            input = "#Test Program \n";
            output = CodeParser.Parse(input);
            expectedOutput = "";
            Assert.That(output, Is.EqualTo(expectedOutput));



        }

    }

}

