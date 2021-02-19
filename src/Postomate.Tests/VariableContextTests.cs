using FluentAssertions;
using Xunit;

namespace Postomate.Tests
{
    public class VariableContextTests
    {
        [Fact]
        public void Replaces_NullValues_With_EmptyString()
        {
            var sut = new MutableVariableContext(new
            {
                NullString = default(string?),
                NullObject = default(object),
                NullVariableContext = default(MutableVariableContext)
            });

            sut.Variables["NullString"].Should().BeEmpty("a null-string should be treated as an empty string");
            sut.Variables["NullObject"].Should().BeEmpty("a null-object should be treated as an empty string");
            sut.Variables["NullVariableContext"].Should().BeEmpty("a null-VariableContext should be treated as an empty string");
        }


        [Fact]
        public void Immutable_VariableContext_Does_Not_Mutate()
        {
            var sut = new ImmutableVariableContext(new
            {
                foo = "bar"
            });

            var enriched = sut.Enrich(new
            {
                bar = "foo"
            });

            sut.Should().NotBe(enriched, "an enriched immutable variable-context should return a fresh instance");

            sut.Variables.Keys.Should().BeEquivalentTo(new[] { "foo" }, "an immutable variable-context should not change at all");
            enriched.Variables.Keys.Should().BeEquivalentTo(new[] { "foo", "bar" }, "when an immutable variable-context is enriched, a fresh instance is generated");
        }

        [Fact]
        public void Mutable_VariableContext_Does_Mutate()
        {
            var sut = new MutableVariableContext(new
            {
                foo = "bar"
            });

            var enriched = sut.Enrich(new
            {
                bar = "foo"
            });

            sut.Should().Be(enriched, "an enriched mutable variable-context should return itself");

            sut.Variables.Keys.Should().BeEquivalentTo(new[] { "foo", "bar" }, "a mutable variable-context should change");
        }
    }
}
