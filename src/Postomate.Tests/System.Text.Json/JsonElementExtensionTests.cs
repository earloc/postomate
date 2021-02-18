using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Postomate.Tests.System.Text.Json
{
    public class JsonElementExtensionTests
    {

        private readonly JsonElement sut;

        public JsonElementExtensionTests()
        {
            var document = JsonDocument.Parse(@"{ ""SomeProperty"" : ""SomeValue"" }");
            sut = document.RootElement;
        }


        [Theory]
        [InlineData("SomeOtherProperty")]
        [InlineData("xyz")]
        public void TryGetProperty_Returns_Null_When_Property_Is_Not_Found(string propertyName)
        {
            var property = sut.TryGetProperty(propertyName);

            property.Should().BeNull("that is the default behavior for the custom-extension");
        }


        [Theory]
        [InlineData("SomeProperty")]
        public void TryGetProperty_Returns_Element_When_Property_Is_Found(string propertyName)
        {
            var property = sut.TryGetProperty(propertyName);

            property.Should().NotBeNull("an existing property should be returned");
        }

        [Theory]
        [InlineData("SomeOtherProperty")]
        [InlineData("xyz")]
        public void RequireProperty_Throws_When_Property_Is_Not_Found(string propertyName)
        {
            sut.Invoking(_ => _.RequireProperty(propertyName))
                .Should().Throw<KeyNotFoundException>()
                .WithMessage($"The property '{propertyName}' was not found on the specified JsonElement");
        }

        [Theory]
        [InlineData("SomeProperty")]
        public void RequireProperty_Returns_Element_When_Property_Is_Found(string propertyName)
        {
            var property = sut.RequireProperty(propertyName);

            property.Should().NotBeNull("an existing property should be returned");
        }
    }
}
