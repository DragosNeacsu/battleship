namespace Battleship.Core.Models;

public class Board
{
    public Board(List<Cell> cells)
    {
        Cells = cells;
        Ships = new List<Ship>();
    }

    public List<Cell> Cells { get; set; }
    public List<Ship> Ships { get; set; }
}