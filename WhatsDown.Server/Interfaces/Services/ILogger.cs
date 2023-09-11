using System.Runtime.CompilerServices;

namespace WhatsDown.Server.Interfaces.Services;

internal interface ILogger
{
    void LogTrace(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
    void LogInformation(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
    void LogWarning(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
    void LogError(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
    void LogFatal(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
}

internal enum LogLevel
{
    Trace,
    Information,
    Warning,
    Error,
    Fatal
}