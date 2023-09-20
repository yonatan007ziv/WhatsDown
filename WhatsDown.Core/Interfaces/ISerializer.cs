namespace WhatsDown.Core.Interfaces;

public interface ISerializer<TMessage>
{
    string? Serialize(TMessage message);
    TMessage? Deserialize(string message);
}