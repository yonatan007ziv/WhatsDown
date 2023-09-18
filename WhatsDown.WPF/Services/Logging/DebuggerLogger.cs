using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.WPF.Services.Logging;

internal class DebuggerLogger : ILogger
{
    public void LogError(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        Debug.WriteLine($"ERROR [{origin}, {lineNumber}]: {message}");
    }

    public void LogFatal(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        Debug.WriteLine($"FATAL: [{origin}, {lineNumber}]: {message}");
    }

    public void LogInformation(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        Debug.WriteLine($"INFORMATION: [{origin}, {lineNumber}]: {message}");
    }

    public void LogSuccess(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        Debug.WriteLine($"SUCCESS: [{origin}, {lineNumber}]: {message}");
    }

    public void LogTrace(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        Debug.WriteLine($"TRACE: [{origin}, {lineNumber}]: {message}");
    }

    public void LogWarning(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        Debug.WriteLine($"WARNING: [{origin}, {lineNumber}]: {message}");
    }
}