using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.WPF.DI.Services.Logging;

class MessageBoxLogger : ILogger
{
    public void LogError(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        MessageBox.Show($"ERROR [{origin}, {lineNumber}]: {message}");
    }

    public void LogFatal(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        MessageBox.Show($"FATAL: [{origin}, {lineNumber}]: {message}");
    }

    public void LogInformation(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        MessageBox.Show($"INFORMATION: [{origin}, {lineNumber}]: {message}");
    }

    public void LogSuccess(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        MessageBox.Show($"SUCCESS: [{origin}, {lineNumber}]: {message}");
    }

    public void LogTrace(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        MessageBox.Show($"TRACE: [{origin}, {lineNumber}]: {message}");
    }

    public void LogWarning(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
    {
        origin = Path.GetFileNameWithoutExtension(origin);
        MessageBox.Show($"WARNING: [{origin}, {lineNumber}]: {message}");
    }
}