using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Postomate
{
    public class UnsubstitutedVariablesException : Exception
    {

        public UnsubstitutedVariablesException(IEnumerable<string> variables) 
            : this("There are unsubstituted variables present in the request", variables)
        {
        }

        public UnsubstitutedVariablesException(string? message, IEnumerable<string> variables) : base($"{message}: {variables.ToJson(false)}")
        {
            Variables = variables;
        }

        public IEnumerable<string> Variables { get; }

    }
}
