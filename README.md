# GPTCSV
![Tests](https://img.shields.io/github/stars/MysteriousTaway/GPTCSV)
![Tests](https://img.shields.io/github/languages/top/MysteriousTaway/GPTCSV)
![Tests](https://img.shields.io/github/forks/MysteriousTaway/GPTCSV)
![Tests](https://img.shields.io/github/issues-pr/MysteriousTaway/GPTCSV)
![Tests](https://img.shields.io/github/last-commit/MysteriousTaway/GPTCSV)<br>
Reads a CSV file with queries which it sends to GPT3 and writes results back in a CSV.

Available arguments:
```
path_input
    Use:      Where the input CSV is located.
    Syntax:   path_input "C:\Your_Location\Input.csv
    Default:  CurrentLocation\input.csv
path_output   
    Use:      Where the output CSV should be located.
    Syntax:   path_output C:\Your_Location\Output.csv
    Default:  CurrentLocation\output.csv
apikey        
    Use:      This is your OpenAI API key
    Syntax:   apikey sk-aaaabbbbccccdddd
    Default:  There's no default.
engine        
    Use:      Which text generation engine should be used
    Options:  davinci, curie, babbage, ada
    Syntax:   engine curie
    Default:  ada
separator     
    Use:      Defines which data separator you use in your CSV
    Options:  tab space newline ; (or you can input a custom one)
    Syntax:   separator space
    Default:  ;
```
