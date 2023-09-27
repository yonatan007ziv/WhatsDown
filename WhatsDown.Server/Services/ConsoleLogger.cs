using System.Runtime.CompilerServices;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Server.Services;

class ConsoleLogger : ILogger
{
	private readonly object lockObj = new object();

	public void LogSuccess(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
	{
		origin = Path.GetFileNameWithoutExtension(origin);
		LogPrefix(LogLevel.Success, origin, lineNumber);
		Console.WriteLine(message);
	}

	public void LogTrace(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
	{
		origin = Path.GetFileNameWithoutExtension(origin);
		LogPrefix(LogLevel.Trace, origin, lineNumber);
		Console.WriteLine(message);
	}

	public void LogInformation(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
	{
		origin = Path.GetFileNameWithoutExtension(origin);
		LogPrefix(LogLevel.Information, origin, lineNumber);
		Console.WriteLine(message);
	}

	public void LogWarning(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
	{
		origin = Path.GetFileNameWithoutExtension(origin);
		LogPrefix(LogLevel.Warning, origin, lineNumber);
		Console.WriteLine(message);
	}

	public void LogError(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
	{
		origin = Path.GetFileNameWithoutExtension(origin);
		LogPrefix(LogLevel.Error, origin, lineNumber);
		Console.WriteLine(message);
	}

	public void LogFatal(string message, [CallerFilePath] string origin = "", [CallerLineNumber] int lineNumber = 0)
	{
		origin = Path.GetFileNameWithoutExtension(origin);
		LogPrefix(LogLevel.Fatal, origin, lineNumber);
		Console.WriteLine(message);
	}

	private void LogPrefix(LogLevel level, string origin, int lineNumber)
	{
		lock (lockObj)
		{
			ConsoleColor originalColor = Console.ForegroundColor;
			ConsoleColor prefixColor;
			switch (level)
			{
				default:
					prefixColor = ConsoleColor.White;
					break;
				case LogLevel.Success:
					prefixColor = ConsoleColor.Green;
					break;
				case LogLevel.Trace:
					prefixColor = ConsoleColor.White;
					break;
				case LogLevel.Information:
					prefixColor = ConsoleColor.Cyan;
					break;
				case LogLevel.Warning:
					prefixColor = ConsoleColor.Yellow;
					break;
				case LogLevel.Error:
					prefixColor = ConsoleColor.Red;
					break;
				case LogLevel.Fatal:
					prefixColor = ConsoleColor.DarkRed;
					break;
			}

			Console.ForegroundColor = prefixColor;
			Console.Write("{0} [class: {1}, line: {2}] - ", level.ToString(), origin, lineNumber);
			Console.ForegroundColor = originalColor;
		}
	}
}