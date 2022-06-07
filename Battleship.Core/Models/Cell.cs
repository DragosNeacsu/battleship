namespace Battleship.Core.Models;

public class Cell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public CellStatus Status { get; set; }
}