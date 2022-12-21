using OpenAI_API;

namespace GPTCSV;
// API: https://github.com/OkGoDoIt/OpenAI-API-dotnet
// I got inspired by: https://github.com/Caerii/GPT3WebSummarizer/blob/main/websum/websum/Gpt3Stuff.cs
public class API {
    private Logger logger = new Logger("API");
    
    private static Configuration configuration = new Configuration();
    private struct Configuration {
        public string APIKey;
        public string engine;
    }
    
    private OpenAIAPI api;
    public API(string APIKey, string engine) {
        try {
            configuration.APIKey = APIKey;
            configuration.engine = engine;
            api = new OpenAIAPI(apiKeys:configuration.APIKey, engine: engine);
        } catch (Exception e) {
            logger.Log("An error occured while initializing the API object: " + e.Message, Logger.LogLevel.Fatal);
            throw;
        }
    }
    
    public async Task<CompletionResult> Completion(APIQuery query) {
        try {
            // Create a completion request
            CompletionRequest request = new CompletionRequest(
                prompt: query.prompt,
                max_tokens: query.max_tokens,
                temperature: query.temperature,
                presencePenalty: query.presence_penalty,
                frequencyPenalty: query.frequency_penalty,
                numOutputs: query.num_outputs
            );
            
            // Send the request to the API
            CompletionResult result = new CompletionResult();
            result = await api.Completions.CreateCompletionAsync(request);
            return result;
        } catch (Exception e) {
            logger.Log("An error occured while completing the prompt: " + e.Message, Logger.LogLevel.Warning);
            logger.Log("Returning empty CompletionResult", Logger.LogLevel.Warning);
            return new CompletionResult();
        }
    }
    
    public async Task<Dictionary<string, double>> Search(APIQuery query) {
        // TODO: FIX ME!!!!!
        try {
            var request = new SearchRequest() {
                Query = query.prompt,
                Documents = query.documents
            };
            // var result = await api.Search.GetSearchResultsAsync(query.prompt, query.documents.ToArray());
            var result = await api.Search.GetSearchResultsAsync(request);
            return result;
        } catch (Exception e) {
            logger.Log("An error occured while completing the prompt: " + e.Message, Logger.LogLevel.Warning);
            logger.Log("Returning empty dictionary", Logger.LogLevel.Warning);
            return new Dictionary<string, double>();
        }
    }
}