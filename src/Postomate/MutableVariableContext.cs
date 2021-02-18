using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postomate
{
    public class MutableVariableContext : VariableContextBase
    {
        public MutableVariableContext(object? context = null, bool requiresFullSubstitution = false)
        {
            if (context is not null)
            {
                Variables = IngestVariables(context);
            }

            RequiresFullSubstitution = requiresFullSubstitution;
        }

        public override IVariableContext Enrich(object context)
        {
            Variables = IngestVariables(context);
            return this;
        }
    }
}
