using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postomate
{
    public class RequestNotFoundException : Exception
    {
        public RequestNotFoundException(string? message) : base(message)
        {
        }
    }
}
