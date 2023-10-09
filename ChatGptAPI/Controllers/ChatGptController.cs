using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;

namespace ChatGptAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatGptController : ControllerBase
    {

        private readonly ILogger<ChatGptController> _logger;

        public ChatGptController(ILogger<ChatGptController> logger)
        {
            _logger = logger;
        }


        [HttpGet(Name = "AskAQuestion")]
        public async Task<string> AskAQuestion([FromHeader] string question)
        {
            var results = "";

            var apiKey = "";

            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = apiKey
            });

            var result = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = question,
                Model = Models.TextDavinciV2,
                Temperature = 0.5F,
                MaxTokens = 100,
                N = 2
            });

            if (result.Successful)
            {
                foreach (var choice in result.Choices)
                {
                    results += choice.Text;
                }
            }
            else
            {
                if (result.Error == null)
                {
                    throw new Exception("Request Failed!");
                }

                results += result.Error.ToString();
            }

            return results;
        }
    }
}
