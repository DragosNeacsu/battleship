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
        //When
        var ship = ShipBuilder.Build(ShipType.Battleship);

        //Then
        ship.Size.Should().Be(5);
    }
    
    [Fact]
    public void ShouldBuildDestroyer()
    {
        //When
        var ship = ShipBuilder.Build(ShipType.Destroyer);

        //Then
        ship.Size.Should().Be(4);
    }
}