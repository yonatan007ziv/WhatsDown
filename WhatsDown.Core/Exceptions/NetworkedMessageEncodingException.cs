using WhatsDown.Core.CommunicationProtocol;

namespace WhatsDown.Core.Exceptions;

internal class NetworkedMessageEncodingException : Exception
{
    public NetworkedMessageEncodingException(MessagePacket msg)
        : base($"Could not Encode Message: {msg}")
    {

    }
}