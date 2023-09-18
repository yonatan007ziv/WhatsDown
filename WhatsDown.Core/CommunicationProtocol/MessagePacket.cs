using System.Text;
using WhatsDown.Core.CommunicationProtocol.Enums;

namespace WhatsDown.Core.CommunicationProtocol;

public class MessagePacket
{
    public CommunicationPurpose Type { get; set; }
    public string[] Param { get; set; }

    public MessagePacket(CommunicationPurpose type, params string[] param)
    {
        Type = type;
        Param = param;
    }

    public TEnum ExtractParam<TEnum>(int i) where TEnum : struct
    {
        return Enum.Parse<TEnum>(Param[i]);
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder($"{Type}:");
        foreach (string s in Param)
            stringBuilder.Append($"{s}, ");
        return stringBuilder.ToString();
    }
}