namespace WhatsDown.Core.Interfaces.Networking.Communication;

public interface IMessageEncoder<TMessage> where TMessage : class
{
    string? EncodeMessage(TMessage message);
    TMessage? DecodeMessage(string message);
}