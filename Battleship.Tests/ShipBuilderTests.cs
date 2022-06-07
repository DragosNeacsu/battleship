using Battleship.Core;
using Battleship.Core.Models;
using FluentAssertions;
using Xunit;

namespace Battleship.Tests;

public class ShipBuilderTests
{
    [Fact]
    public void ShouldBuildBattleship()
    {
        //Given
        var shipBuilder = new ShipBuilder();

        //When
        var ship = shipBuilder.Build(ShipType.Battleship);

        //Then
        ship.Size.Should().Be(5);
    }
    
    [Fact]
    public void ShouldBuildDestroyer()
    {
        //Given
        var shipBuilder = new ShipBuilder();

        //When
        var ship = shipBuilder.Build(ShipType.Destroyer);

        //Then
        ship.Size.Should().Be(4);
    }
}