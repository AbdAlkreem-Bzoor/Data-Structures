namespace DS.SortedDictionary.Exceptions;

public sealed class EmptyDictionaryException(string message) : Exception(message)
{
    public override string Message => base.Message;
}