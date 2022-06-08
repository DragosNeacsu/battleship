using Battleship.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace Battleship.Tests;

public class CoordinatesExtensionTests
{
    [Theory]
    [InlineData("A1", 1, 1)]
    [InlineData("A5", 1, 5)]
    [InlineData("B5", 2, 5)]
    [InlineData("e16", 5, 16)]
    [InlineData("aa16", 1, 16)]
    public void ShouldConvertCoordinatesToNumbers(string value, int expectedColumn, int expectedRow)
    {
        //When
        var row = CoordinatesExtension.GetRow(value);
        var column = CoordinatesExtension.GetColumn(value);


        //Then
        row.Should().Be(expectedRow);
        column.Should().Be(expectedColumn);
    }
}