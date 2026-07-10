namespace DS.SortedDictionary.Exceptions;

public sealed class FoundKeyException(string message) : Exception(message)
{
    public override string Message => base.Message;
}