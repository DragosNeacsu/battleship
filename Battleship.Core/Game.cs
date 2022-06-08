using Battleship.Core.Models;

namespace Battleship.Core;

public class Game
{
    public static Board PlaceShip(Board board, Ship ship, int row, int column)
    {
        TryValidate(board, ship, row, column);

        var shipCells = board.Cells.Where(c =>
            c.Row == row && Enumerable.Range(column, ship.Size).Any(x => x == c.Column)).ToList();

        if (shipCells.Any(x => x.Status != CellStatus.Water))
        {
            throw new Exception("Can't place ship at this location");
        }

        shipCells.ForEach(c => c.Status = CellStatus.Ship);
        ship.Column = column;
        ship.Row = row;

        board.Ships.Add(ship);

        return board;
    }

    public static Board Attack(Board board, int row, int column)
    {
        ValidateInsideBoard(board, row, column);

        var cell = board.Cells.First(c => c.Row == row && c.Column == column);

        cell.Status = cell.Status switch
        {
            CellStatus.Hit => CellStatus.Hit,
            CellStatus.Ship => CellStatus.Hit,
            CellStatus.Sink => CellStatus.Sink,
            _ => CellStatus.Miss
        };

        return CheckShip(board, row, column);
    }

    private static Board CheckShip(Board board, int row, int column)
    {
        foreach (var ship in board.Ships.Where(s => !s.Sunk))
        {
            var shipCells = board.Cells.Where(c =>
                c.Row == row && Enumerable.Range(ship.Column, ship.Size).Any(x => x == c.Column)).ToList();

            if (shipCells.All(c => c.Status == CellStatus.Hit))
            {
                ship.Sunk = true;
                shipCells.ForEach(c => c.Status = CellStatus.Sink);
            }
        }
        return board;
    }

    private static void TryValidate(Board board, Ship ship, int row, int column)
    {
        ValidateInsideBoard(board, row, column);

        if (column + ship.Size > board.Cells.Max(c => c.Column))
        {
            throw new IndexOutOfRangeException($"Ship will not fit at {row},{column}");
        }
    }

    private static void ValidateInsideBoard(Board board, int row, int column)
    {
        if (row > board.Cells.Max(c => c.Row) ||
            row < board.Cells.Min(c => c.Row) ||
            column > board.Cells.Max(c => c.Column) ||
            column < board.Cells.Min(c => c.Column))
        {
            throw new IndexOutOfRangeException("Position outside the board");
        }
    }
}