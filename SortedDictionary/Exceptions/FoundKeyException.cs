namespace SortedDictionary.Exceptions;

public sealed class FoundKeyException : Exception
{
    public FoundKeyException(string message) : base(message) { }
    public override string Message => base.Message;
}