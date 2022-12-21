namespace GPTCSV; 

public class CSV {
    private static Logger logger = new Logger("CSV");
    public static List<string?> Read(string path) {
        try {
            List<string?> data = new List<string?>();
            using(var reader = new StreamReader(path)) {
                while (!reader.EndOfStream) {
                    data.Add(reader.ReadLine());
                }
            }
            // Filter out comments
            for (int i = 0; i < data.Count; i++) {
                if (data[i].StartsWith("#")) {
                    data.RemoveAt(i);
                    i--;
                }
            }
            return data;
        } catch (Exception e) {
            logger.Log("Error while reading CSV file: " + e.Message, Logger.LogLevel.Error);
            logger.Log("Returning empty List", Logger.LogLevel.Warning);
            return new List<string?>();
        }
    }
    
    public static void Write(string path, List<APIResult> data, string separator) {
        try {
            if (!File.Exists(path)) File.Create(path);
            int i = 0;
            foreach (APIResult? result in data) {
                if (result.type.ToLower().Equals("search")) {
                    // TODO: TEST THIS. (After you fix the API Search method)
                    string searches = "";
                    foreach (var search in result.searchResult) {
                        searches += $"{search.Key}:{search.Value}{separator}";
                        logger.Log($"Writing: {search.Key}:{search.Value}{separator} ", Logger.LogLevel.Debug);
                    }
                    searches += separator;
                    File.AppendAllText(path,searches);
                } else if (result.type.ToLower().Equals("completion")) {
                    string choices = "";
                    foreach (var choice in result.completionResult.Completions) {
                        choices += $"{choice.Index}:{choice.Text}{separator}";
                    }
                    string completions =
                        $"results:{{{choices}}}{separator}" +
                        $"created:{result.completionResult.Created}{separator}" +
                        $"id:{result.completionResult.Id}{separator}" +
                        $"model:{result.completionResult.Model}{separator}" +
                        $"organization:{result.completionResult.Organization}{separator}" +
                        $"processing_time:{result.completionResult.ProcessingTime}{separator}" +
                        $"request_id:{result.completionResult.RequestId}{separator}" +
                        $"created_unix_time:{result.completionResult.CreatedUnixTime}{separator}";
                    logger.Log($"Writing: {completions}", Logger.LogLevel.Debug);
                    File.AppendAllText(path,completions);
                } else {
                    logger.Log($"Unknown type {result.type} at index {i}", Logger.LogLevel.Warning);
                }
                i++;
            }
        } catch (Exception e) {
            logger.Log("Error while writing CSV file: " + e.Message, Logger.LogLevel.Error);
        }
    }
    
    public static List<APIQuery> ConvertToQueryList(List<string?> data, string separator) {
        try {
            List<APIQuery> queries = new List<APIQuery>();
            foreach (var line in data) {
                string[] split = line.Split(separator);
                APIQuery apiquery = new APIQuery();
                foreach (var arg in split) {
                    if(arg.Contains("prompt:")){
                        apiquery.prompt = arg.Replace("prompt:", "");
                    } else if(arg.Contains("max_tokens:")){
                        apiquery.max_tokens = int.Parse(arg.Replace("max_tokens:", ""));
                    } else if(arg.Contains("temperature:")){
                        apiquery.temperature = float.Parse(arg.Replace("temperature:", ""));
                    } else if(arg.Contains("presence_penalty:")){
                        apiquery.presence_penalty = float.Parse(arg.Replace("presence_penalty:", ""));
                    } else if(arg.Contains("frequency_penalty:")){
                        apiquery.frequency_penalty = float.Parse(arg.Replace("frequency_penalty:", ""));
                    } else if(arg.Contains("num_outputs:")){
                        apiquery.num_outputs = int.Parse(arg.Replace("num_outputs:", ""));
                    } else if (arg.Contains("operation:")) {
                        apiquery.operation = arg.Replace("operation:", "");
                    } else if (arg.Contains("documents:")) {
                        List<string> docs = new List<string>();
                        foreach (var doc in arg.Replace("documents:", "").Split(",")) {
                            docs.Add(doc);
                        }
                        apiquery.documents = docs;
                    }
                }
                queries.Add(apiquery);
            }
            return queries;
        } catch(Exception e) {
            logger.Log("Error while converting CSV data to APIQuery list: " + e.Message, Logger.LogLevel.Error);
            logger.Log("Returning empty List", Logger.LogLevel.Warning);
            return new List<APIQuery>();
        }
    }
}