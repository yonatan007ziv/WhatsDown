namespace WhatsDown.Core.Exceptions;

public class NetworkedWriteException : Exception
{
	public WriteExceptionType Type { get; }

	public NetworkedWriteException(WriteExceptionType type)
		: base()
	{
		Type = type;
	}
}

public enum WriteExceptionType
{
	SerializationFailed,
	TextEncodingFailed,
	EncryptionFailed,
	IO,
	Disposed,
	Timedout,
	OperationCancelled,
}