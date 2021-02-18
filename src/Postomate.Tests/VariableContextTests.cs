using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Postomate.Tests
{
    public class VariableContextTests
    {
        [Fact]
        public void Replaces_NullValues_With_EmptyString()
        {
            var sut = new VariableContext(new
            {
                NullString = default(string?),
                NullObject = default(object),
                NullVariableContext = default(VariableContext)
            });

            sut.Variables["NullString"].Should().BeEmpty("a null-string should be treated as an empty string");
            sut.Variables["NullObject"].Should().BeEmpty("a null-object should be treated as an empty string");
            sut.Variables["NullVariableContext"].Should().BeEmpty("a null-VariableContext should be treated as an empty string");

        }
    }
}
