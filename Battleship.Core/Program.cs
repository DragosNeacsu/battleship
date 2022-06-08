// See https://aka.ms/new-console-template for more information

using Battleship.Core.Extensions;
using Battleship.Core.Models;

namespace Battleship.Core
{
    internal static class Program
    {
        private static int RowsCount => 10;
        private static int ColumnsCount => 10;

        static void Main(string[] args)
        {
            var board = BoardBuilder.Create(RowsCount, ColumnsCount);
            var game = new Game();

            var numberOfShips = 4;
            for (var i = 0; i < numberOfShips; i++)
            {
                var ship = ShipBuilder.Build(RandomExtensions.RandomEnum<ShipType>());
                board = PlaceShipRandomly(board, ship, game);
            }

            DisplayBoard(board);
            var attackPosition = Console.ReadLine();
            while ((attackPosition != null && attackPosition != "exit"))
            {
                try
                {
                    board = Game.Attack(board, CoordinatesExtension.GetRow(attackPosition),
                        CoordinatesExtension.GetColumn(attackPosition));
                    DisplayBoard(board);
                    if (board.Ships.All(s => s.Sunk == true))
                    {
                        Console.WriteLine("You win!!!");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                attackPosition = Console.ReadLine();
            }
        }

        private static void DisplayBoard(Board board)
        {
            Console.Clear();
            WriteColumnHeader(board);
            foreach (var cell in board.Cells)
            {
                if (cell.Column == 1)
                {
                    Console.Write(cell.Row);
                }

                switch (cell.Status)
                {
                    case CellStatus.Ship:
                        Console.Write("[ ]");
                        break;
                    case CellStatus.Hit:
                        Console.Write("[*]");
                        break;
                    case CellStatus.Sink:
                        Console.Write("[x]");
                        break;
                    case CellStatus.Miss:
                        Console.Write(" 0 ");
                        break;
                    case CellStatus.Water:
                        Console.Write(" ~ ");
                        break;
                }

                if (cell.Column == ColumnsCount)
                {
                    Console.WriteLine();
                }
            }
        }

        private static void WriteColumnHeader(Board board)
        {
            var columnsCount = board.Cells.Max(c => c.Column);
            var header = " ";
            for (var i = 1; i <= columnsCount; i++)
            {
                header += $" {Convert.ToChar(i + 64)} ";
            }

            Console.WriteLine(header);
        }

        private static Board PlaceShipRandomly(Board board, Ship ship, Game game)
        {
            var randomGenerator = new Random();
            try
            {
                return Game.PlaceShip(board, ship, randomGenerator.Next(1, RowsCount + 1),
                    randomGenerator.Next(1, ColumnsCount + 1));
            }
            catch (Exception)
            {
                return PlaceShipRandomly(board, ship, game);
            }
        }
    }
}