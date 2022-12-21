namespace GPTCSV; 

public class APIQuery {
    public string prompt;
    public int max_tokens;
    public float temperature;
    public float presence_penalty;
    public float frequency_penalty;
    public int num_outputs;
    public string operation;
    public List<string> documents;

    public APIQuery(){}

    public override string ToString() {
        return $"{nameof(prompt)}: {prompt}, {nameof(max_tokens)}: {max_tokens}, {nameof(temperature)}: {temperature}, {nameof(presence_penalty)}: {presence_penalty}, {nameof(frequency_penalty)}: {frequency_penalty}, {nameof(num_outputs)}: {num_outputs}, {nameof(operation)}: {operation}";
    }

    public string ToStringNL() {
        return $"{nameof(prompt)}: {prompt}\n {nameof(max_tokens)}: {max_tokens}\n {nameof(temperature)}: {temperature}\n {nameof(presence_penalty)}: {presence_penalty}\n {nameof(frequency_penalty)}: {frequency_penalty}\n {nameof(num_outputs)}: {num_outputs}\n {nameof(operation)}: {operation}";
    }

    public string Prompt {
        get => prompt;
        set => prompt = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int MaxTokens {
        get => max_tokens;
        set => max_tokens = value;
    }

    public float Temperature {
        get => temperature;
        set => temperature = value;
    }

    public float PresencePenalty {
        get => presence_penalty;
        set => presence_penalty = value;
    }

    public float FrequencyPenalty {
        get => frequency_penalty;
        set => frequency_penalty = value;
    }

    public int NumOutputs {
        get => num_outputs;
        set => num_outputs = value;
    }
    
    public string Operation {
        get => operation;
        set => operation = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<string> Documents {
        get => documents;
        set => documents = value ?? throw new ArgumentNullException(nameof(value));
    }
}