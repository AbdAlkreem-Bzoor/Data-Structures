namespace DS.SortedDictionary.Exceptions;

public sealed class NotFoundKeyException(string message) : Exception(message)
{
    public override string Message => base.Message;
}