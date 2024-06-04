namespace AutomateUnitTestChatGPT.Service
{

    public class OpenAIService
    {
        //private readonly OpenAI openai;
        private string output;

        public OpenAIService(IConfiguration configuration)
        {
            string apiKey = configuration["OpenAI:ApiKey"]; // Load API key from configuration file
            //openai = new OpenAIApi(apiKey);
        }

        private string UNIT_TEST_REQUEST(string framework, string path, string code)
        {
            return $"Generate a unit test with the {framework} syntax, containing relevant assertions and required packages in a single 'describe' block. Import the functions from {path} and use them to test the following code snippet: {code}.";
        }

        public async Task<string> ReadFileAsCodeAsync(string filePath)
        {
            try
            {
                return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading file: {ex.Message}");
            }
        }

        public async Task GenerateUnitTestAsync(string framework, string path, string code)
        {
            try
            {
                Console.WriteLine("Please Wait Generating unit test...");
                //var response = await openai.CreateChatCompletionAsync(new ChatCompletionRequest
                //{
                //    Model = "gpt-3.5-turbo",
                //    Messages = new[] { new ChatMessage { Role = "user", Content = UNIT_TEST_REQUEST(framework, path, code) } },
                //    Temperature = 0,
                //    MaxTokens = 1000
                //});

                //output = response.Choices[0].Message.Content;

                Console.WriteLine("Unit test generated");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating unit test: {ex.Message}");
            }
        }

        public async Task CreateTestSuiteFileAsync()
        {
            string fileName = "./src/unitTestSuie.js"; // Adjust file path as needed
            try
            {
                await File.WriteAllTextAsync(fileName, output);
                Console.WriteLine($"Message written to file: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
