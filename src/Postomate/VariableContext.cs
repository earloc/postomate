using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postomate
{
    public class VariableContext
    {
        public VariableContext(object? context = null)
        {
            if (context is not null)
            {
                Enrich(context);
            }
        }

        public IEnumerable<KeyValuePair<string, string>> Variables { get; private set; } = Enumerable.Empty<KeyValuePair<string, string>>();

        public VariableContext Enrich(object context)
        {
            var moreVariables = new Dictionary<string, string>(Variables.ToArray());

            foreach (var property in context.GetType().GetProperties())
            {
                moreVariables[property.Name] = property.GetValue(context)?.ToString() ?? "";
            }

            Variables = moreVariables;

            return this;
        }
    }


}
