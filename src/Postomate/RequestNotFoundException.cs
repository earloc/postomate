using System;

namespace Postomate
{
    public class RequestNotFoundException : Exception
    {
        public RequestNotFoundException(string? message) : base(message)
        {
        }
    }
}
