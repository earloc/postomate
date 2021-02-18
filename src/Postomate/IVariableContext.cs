using System.Collections.Generic;

namespace Postomate
{
    public interface IVariableContext
    {
        bool RequiresFullSubstitution { get; }
        IDictionary<string, string> Variables { get; }

        IVariableContext Enrich(object context);
    }
}