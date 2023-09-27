namespace WhatsDown.Core.Exceptions;

public class SerializationException : Exception
{
	public SerializationException(object deserialized)
		: base($"Exception while Serializing: {deserialized}")
	{

	}
}