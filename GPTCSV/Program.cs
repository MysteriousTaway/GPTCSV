using GPTCSV;
using OpenAI_API;
/* Program made by https://taway.dev/ */
class Program {
    private static Logger logger = new Logger("Program");

    public static bool DEBUG = true;
    private static ARGS arguments = new ARGS();
    private struct ARGS {
        public string path_input;
        public string path_output;
        public string APIKey;
        public Engine engine;
        public string separator;
    }
    static void Main(string[] args) {
        #region NoArgs
        if (args.Length == 0) {
            logger.Log("Available arguments:");
            logger.Log("path_input    ");
            logger.Log("    Use:      Where the input CSV is located.");
            logger.Log("    Syntax:   path_input \"C:\\Your_Location\\Input.csv\"");
            logger.Log("    Default:  CurrentLocation\\input.csv");
            logger.Log("path_output   ");
            logger.Log("    Use:      Where the output CSV should be located.");
            logger.Log("    Syntax:   path_output \"C:\\Your_Location\\Output.csv\"");
            logger.Log("    Default:  CurrentLocation\\output.csv");
            logger.Log("apikey        ");
            logger.Log("    Use:      This is your OpenAI API key");
            logger.Log("    Syntax:   apikey sk-aaaabbbbccccdddd");
            logger.Log("    Default:  There's no default.");
            logger.Log("engine        ");
            logger.Log("    Use:      Which text generation engine should be used");
            logger.Log("    Options:  davinci, curie, babbage, ada");
            logger.Log("    Syntax:   engine curie");
            logger.Log("    Default:  ada");
            logger.Log("separator     ");
            logger.Log("    Use:      Defines which data separator you use in your CSV");
            logger.Log("    Options:  tab space newline ; (or you can input a custom one)");
            logger.Log("    Syntax:   separator space");
            logger.Log("    Default:  ;");
        }
        #endregion
        
        // A few default values
        arguments.path_input = "input.csv";
        arguments.path_output = "output.csv";
        arguments.engine = "text-ada-001";
        arguments.separator = ";";

        // Parse arguments
        logger.Log("Parsing arguments");
        try {
            for (int i = 0; i < args.Length; i++) {
                string arg = args[i].Replace("-", "");
                switch (args[i]) {
                    case("path_input"):
                        arguments.path_input = args[i + 1];
                        logger.Log($"path_input set to {arguments.path_input}", Logger.LogLevel.Debug);
                        break;
                    case("path_output"):
                        arguments.path_output = args[i + 1];
                        logger.Log($"path_output set to {arguments.path_output}", Logger.LogLevel.Debug);
                        break;
                    case("apikey"):
                    case("api_key"):
                        arguments.APIKey = args[i + 1];
                        logger.Log($"apikey set to {arguments.APIKey}", Logger.LogLevel.Debug);
                        break;
                    case("engine"):
                        if (args[i + 1].ToLower().Contains("davinci")) {
                            arguments.engine = Engine.Davinci;
                        } else if (args[i + 1].ToLower().Contains("curie")) {
                            arguments.engine = Engine.Curie;
                        } else if (args[i + 1].ToLower().Contains("babbage")) {
                            arguments.engine = Engine.Babbage;
                        } else if (args[i + 1].ToLower().Contains("ada")) {
                            arguments.engine = Engine.Ada;
                        } else {
                            throw new Exception("Invalid engine! Please use one of the following: davinci, curie, babbage, ada");
                        }
                        logger.Log($"engine set to {arguments.engine}", Logger.LogLevel.Debug);
                        break;
                    case("separator"):
                        if (args[i].Replace(" ", "").ToLower().Equals("tab")) {
                            arguments.separator = "\t";
                        } else if (args[i].Replace(" ", "").ToLower().Equals("space")) {
                            arguments.separator = " ";
                        } else if (args[i].Replace(" ", "").ToLower().Equals("newline")) {
                            arguments.separator = " ";
                        } else if (args[i].Replace(" ", "").ToLower().Equals("#")) {
                            throw new Exception("# is a comment character and cannot be used as a separator!");
                        } else if (args[i].Replace(" ", "").ToLower().Equals(",")) {
                            throw new Exception(", is a document separator and cannot be used as a normal separator!");
                        } else {
                            arguments.separator = args[i + 1];
                        }
                        logger.Log($"separator set to {arguments.separator}", Logger.LogLevel.Debug);
                        break;
                }
            }
        } catch (Exception e) {
            Console.WriteLine("Error parsing arguments: " + e.Message, Logger.LogLevel.Fatal);
            throw;
        }
        
        // Convert CSV to APIQuery List
        logger.Log("Converting CSV data to APIQuery List");
        List<APIQuery> queries = new List<APIQuery>();
        try {
            queries = CSV.ConvertToQueryList(CSV.Read(arguments.path_input), arguments.separator);
        } catch (Exception e) {
            logger.Log("Error reading CSV and converting it to a APIQuery List: " + e.Message, Logger.LogLevel.Error);
        }
        
        // Write out all queries
        if (DEBUG) {
            for(int i = 0; i < queries.Count; i++) {
                logger.Log($"[QUERY {i}] with data: [{queries[i].ToString()}]", Logger.LogLevel.Debug);
            }
        }

        // Send APIQuery List to API
        logger.Log("Contacting API");
        List<APIResult> results = new List<APIResult>();
        try {
            API api = new API(arguments.APIKey, arguments.engine);
            int i = 1;
            foreach (APIQuery query in queries) {
                logger.Log($"Running query {i} of {queries.Count}");
                CompletionResult completionResult;
                Dictionary<string, double> searchResult;
                switch (query.operation.ToLower().Replace(" ", "")) {
                    case("search"):
                        searchResult = api.Search(query).Result;
                        results.Add(new APIResult("search", searchResult));
                        foreach (var sr in searchResult) {
                            logger.Log($"[SEARCH {i}] KEY: {sr.Key} VALUE: {sr.Value}", Logger.LogLevel.Debug);
                        }
                        break;
                    case("completion"):
                        completionResult = api.Completion(query).Result;
                        results.Add(new APIResult("completion", completionResult));
                        logger.Log($"[COMPLETION {i}] USING ENGINE: {completionResult.Model.EngineName} PROCESSING TIME: {completionResult.ProcessingTime} REQUEST ID: {completionResult.RequestId}", Logger.LogLevel.Debug);
                        logger.Log($"[COMPLETION {i}] QUERY RESULT: {completionResult.ToString()}", Logger.LogLevel.Debug);
                        break;
                }
                i++;
            }
        } catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        logger.Log("Writing results to CSV");
        try {
            CSV.Write(arguments.path_output, results, arguments.separator);
        } catch (Exception e) {
            logger.Log("Error writing results to CSV: " + e.Message, Logger.LogLevel.Error);
        }
    }
}