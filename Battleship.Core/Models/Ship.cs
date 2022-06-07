namespace Battleship.Core.Models;

public class Ship
{
    public Ship(int size)
    {
        Size = size;
    }

    public int Size { get; set; }
    public int Column { get; set; }
    public int Row { get; set; }
    public bool Sunk { get; set; }
}