namespace SortedDictionary.Exceptions;

public sealed class EmptyDictionaryException : Exception
{
    public EmptyDictionaryException(string message) : base(message) { }
    public override string Message => base.Message;
}