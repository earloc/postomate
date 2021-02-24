using System.Collections.Generic;

namespace Postomate
{
    public abstract class VariableContextBase : IVariableContext
    {
        public bool RequiresFullSubstitution { get; protected set; } = false;

        public IDictionary<string, string> Variables { get; protected set; } = new Dictionary<string, string>();

        protected Dictionary<string, string> IngestVariables(object context)
        {
            var moreVariables = new Dictionary<string, string>(Variables);

            foreach (var property in context.GetType().GetProperties())
            {
                moreVariables[property.Name] = property.GetValue(context)?.ToString() ?? "";
            }

            return moreVariables;
        }

        public abstract IVariableContext Enrich(object context);
    }
}
