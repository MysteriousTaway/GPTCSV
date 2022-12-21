namespace GPTCSV; 

public class Logger {
    public enum LogLevel {
        Debug = ConsoleColor.Blue,
        Info = ConsoleColor.White,
        Warning = ConsoleColor.Yellow,
        Error = ConsoleColor.Red,
        Fatal = ConsoleColor.DarkRed
    }
    private string name;
    public Logger(string name) {
        this.name = name;
    }
    
    public void Log(string message, LogLevel level = LogLevel.Info) {
        if (!Program.DEBUG && level == LogLevel.Debug) return;
        Console.ForegroundColor = (ConsoleColor)level;
        Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] [{level.ToString()}] [{name}] {message}");
        Console.ResetColor();
    }
}