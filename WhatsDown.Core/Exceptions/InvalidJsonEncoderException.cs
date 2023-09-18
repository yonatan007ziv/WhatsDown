namespace WhatsDown.Core.Exceptions;

internal class InvalidJsonEncoderException : Exception
{
    public InvalidJsonEncoderException(string operation)
        : base($"Invalid Json Operation: {operation}")
    {

    }
}