
using AIV4.Shared;


namespace AIV4.Tests
{


    [TestFixture]
    public class CodeItTests
    {



        [Test]
        public void Format_StartOfCodeBlock_ShouldReturnOpeningTags()
        {
            // Arrange
            string input = "```html\n";
            string expectedOutput = "<pre>\n<code class='language-html'>";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void Format_CodeBlockContent_ShouldReturnContentWithNoModification()
        {
            // Arrange
            CodeIt.Format("```html\n"); // Start of code block
            string input = "<div>\n    console.log('Hello World');\n</div>\n";
            string expectedOutput = "<div>\n    console.log('Hello World');\n</div>\n";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Format_EndOfCodeBlock_ShouldReturnClosingTags()
        {
            // Arrange
            CodeIt.Format("```html\n"); // Start of code block
            string input = "</div>\n```";
            string expectedOutput = "</code>\n</pre>";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Format_CodeBlockWithExtraNewlines_ShouldHandleExtraNewlines()
        {
            // Arrange
            CodeIt.Format("```html\n"); // Start of code block
            string input = "<div>\n\n    console.log('Hello World');\n\n</div>\n\n```";
            string expectedOutput = "<div>\n\n    console.log('Hello World');\n\n</div>\n\n</code>\n</pre>";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Format_LanguageNameWithExtraBackticks_ShouldHandleExtraBackticks()
        {
            // Arrange
            string input = "```javasc```ript\n";
            string expectedOutput = "<pre>\n<code class='language-javascript'>";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Format_EmptyString_ShouldReturnEmpty()
        {
            // Arrange
            string input = "";
            string expectedOutput = "";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Format_CodeWithMarkup_ShouldReplaceAngleBrackets()
        {
            // Arrange
            CodeIt.Format("```markup\n"); // Start of code block
            string input = "<div>\n    <span>Hello</span>\n</div>\n```";
            string expectedOutput = "&lt;div&gt;\n    &lt;span&gt;Hello&lt;/span&gt;\n&lt;/div&gt;\n</code>\n</pre>";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Format_CodeWithJavascript_ShouldNotReplaceAngleBrackets()
        {
            // Arrange
            CodeIt.Format("```javascript\n"); // Start of code block
            string input = "<div>\n    console.log('Hello World');\n</div>\n```";
            string expectedOutput = "<div>\n    console.log('Hello World');\n</div>\n</code>\n</pre>";

            // Act
            string result = CodeIt.Format(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }
    }

}