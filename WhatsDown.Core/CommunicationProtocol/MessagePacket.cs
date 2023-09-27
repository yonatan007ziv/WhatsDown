using System.Text;
using WhatsDown.Core.CommunicationProtocol.Enums;

namespace WhatsDown.Core.CommunicationProtocol;

public class MessagePacket
{
	// Flags
	public bool InvalidPacket { get; set; }
	public bool TimedOut { get; set; }

	public CommunicationType Type { get; set; }
	public string[] Params { get; set; } = Array.Empty<string>();

	public MessagePacket() { } // Used For JSON Deserialization

	public MessagePacket(CommunicationType type, params object[] parameters)
	{
		Type = type;
		Params = parameters.Select(e => e.ToString()!).ToArray();
	}

	public T ExtractParamAsEnum<T>(int i) where T : struct
	{
		try
		{
			return Enum.Parse<T>(Params[i]);
		}
		catch { return default; }
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder($"{Type}:");
		foreach (string s in Params)
			stringBuilder.Append($"{s}, ");
		return stringBuilder.ToString();
	}
}