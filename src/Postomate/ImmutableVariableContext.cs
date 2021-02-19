using System.Collections.Generic;

namespace Postomate
{

    public class ImmutableVariableContext : VariableContextBase
    {

        public ImmutableVariableContext(IDictionary<string, string> variables, bool requiresFullSubstitution = false)
        {
            Variables = variables;
            RequiresFullSubstitution = requiresFullSubstitution;
        }

        public ImmutableVariableContext(object? context = null, bool requiresFullSubstitution = false)
        {
            if (context is not null)
            {
                Variables = IngestVariables(context);
            }

            RequiresFullSubstitution = requiresFullSubstitution;
        }

        public override IVariableContext Enrich(object context)
        {
            var moreVariables = IngestVariables(context);

            return new ImmutableVariableContext(moreVariables);
        }

        
    }


}
