using Battleship.Core.Models;

namespace Battleship.Core;

public class ShipBuilder
{
    public Ship Build(ShipType shipType) =>
        shipType switch
        {
            ShipType.Battleship => new Ship(5),
            ShipType.Destroyer => new Ship(4),
            _ => new Ship(0)
        };
}