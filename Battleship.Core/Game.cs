using Battleship.Core.Models;

namespace Battleship.Core;

public class Game
{
    public Board PlaceShip(Board board, Ship ship, int row, int column)
    {
        TryValidate(board, ship, row, column);

        var shipCells = board.Cells.Where(c =>
            c.Row == row && Enumerable.Range(column, ship.Size).Any(x => x == c.Column)).ToList();

        if (shipCells.Any(x => x.Status != CellStatus.Water))
        {
            throw new Exception("Cant's place ship at this location");
        }

        shipCells.ForEach(c => c.Status = CellStatus.Ship);
        ship.Column = column;
        ship.Row = row;

        board.Ships.Add(ship);

        return board;
    }

    public Board Attack(Board board, int row, int column)
    {
        if (row > board.Cells.Max(c => c.Row) ||
            row < board.Cells.Min(c => c.Row) ||
            column > board.Cells.Max(c => c.Column) ||
            column < board.Cells.Min(c => c.Column))
        {
            throw new IndexOutOfRangeException("Attack outside the board");
        }

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

    private Board CheckShip(Board board, int row, int column)
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

    private void TryValidate(Board board, Ship ship, int row, int column)
    {
        if (row > board.Cells.Max(c => c.Row) ||
            row < board.Cells.Min(c => c.Row) ||
            column > board.Cells.Max(c => c.Column) ||
            column < board.Cells.Min(c => c.Column))
        {
            throw new IndexOutOfRangeException("Place ship inside the board");
        }

        if (column + ship.Size > board.Cells.Max(c => c.Column))
        {
            throw new IndexOutOfRangeException($"Ship will not fit at {row},{column}");
        }
    }
}