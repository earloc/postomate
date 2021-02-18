using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postomate
{
    public class MutableVariableContext : IVariableContext
    {
        public MutableVariableContext(object? context = null, bool requiresFullSubstitution = false)
        {
            if (context is not null)
            {
                Enrich(context);
            }
            RequiresFullSubstitution = requiresFullSubstitution;
        }

        public IDictionary<string, string> Variables { get; private set; } = new Dictionary<string, string>();

        public bool RequiresFullSubstitution { get; }

        public MutableVariableContext Enrich(object context)
        {
            var moreVariables = new Dictionary<string, string>(Variables);

            foreach (var property in context.GetType().GetProperties())
            {
                moreVariables[property.Name] = property.GetValue(context)?.ToString() ?? "";
            }

            Variables = moreVariables;

            return this;
        }
    }


}
