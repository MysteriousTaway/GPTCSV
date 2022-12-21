using OpenAI_API;

namespace GPTCSV; 

public class APIResult {
    public CompletionResult completionResult;
    public Dictionary<string, double> searchResult;
    public string type;

    public APIResult(string type) {
        this.type = type;
    }

    public APIResult(string type, CompletionResult completionResult) {
        this.completionResult = completionResult;
        this.type = type;
    }

    public APIResult(string type, Dictionary<string, double> searchResult) {
        this.searchResult = searchResult;
        this.type = type;
    }

    public CompletionResult CompletionResult {
        get => completionResult;
        set => completionResult = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Dictionary<string, double> SearchResult {
        get => searchResult;
        set => searchResult = value ?? throw new ArgumentNullException(nameof(value));
    }
}