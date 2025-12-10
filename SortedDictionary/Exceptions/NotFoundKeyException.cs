using System;
using System.Collections.Generic;
using System.Text;

namespace SortedDictionary.Exceptions;

public sealed class NotFoundKeyException : Exception
{
    public NotFoundKeyException(string message) : base(message) { }
    public override string Message => base.Message;
}
