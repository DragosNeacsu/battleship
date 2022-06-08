using Battleship.Core.Models;

namespace Battleship.Core;

public class BoardBuilder
{
    public static Board Create(int rows, int columns)
    {
        if (rows <= 5 || columns <= 5)
        {
            throw new ArgumentException("Invalid board size");
        }

        var cells = new List<Cell>();
        for (var i = 1; i <= rows; i++)
        {
            for (var j = 1; j <= columns; j++)
            {
                cells.Add(new Cell { Row = i, Column = j, Status = CellStatus.Water});
            }
        }

        return new Board(cells);
    }
}