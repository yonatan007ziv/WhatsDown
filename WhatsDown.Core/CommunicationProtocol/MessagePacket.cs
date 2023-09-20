using System.Text;
using WhatsDown.Core.CommunicationProtocol.Enums;

namespace WhatsDown.Core.CommunicationProtocol;

public class MessagePacket
{
    public CommunicationValid Result { get; set; } = CommunicationValid.Yes;
    public CommunicationType Type { get; set; }
    public string[] Params { get; set; } = Array.Empty<string>();

    public MessagePacket() { } // Used For JSON Deserialization

    public MessagePacket(CommunicationType _type, params object[] parameters)
    {
        Type = _type;
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