using System.Runtime.CompilerServices;

namespace WhatsDown.Core.Interfaces;

public interface ILogger
{
	void LogSuccess(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
	void LogTrace(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
	void LogInformation(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
	void LogWarning(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
	void LogError(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
	void LogFatal(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0);
}

public enum LogLevel
{
	Success,
	Trace,
	Information,
	Warning,
	Error,
	Fatal
}