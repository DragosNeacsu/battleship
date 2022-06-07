// See https://aka.ms/new-console-template for more information

using Battleship.Core.Extensions;
using Battleship.Core.Models;

namespace Battleship.Core
{
    internal class Program
    {
        private static int _rowsCount => 10;
        private static int _columnsCount => 10;

        static void Main(string[] args)
        {
            var board = new BoardBuilder().Create(_rowsCount, _columnsCount);
            var game = new Game();

            var numberOfShips = 5;
            for (int i = 0; i < numberOfShips; i++)
            {
                var ship = new ShipBuilder().Build(RandomExtensions.RandomEnum<ShipType>());
                board = PlaceShipRandomly(board, ship, game);
            }

            DisplayBoard(board);
            var attackPosition = Console.ReadLine();
            while ((attackPosition != null && attackPosition != "exit") || board.Ships.Any(s => s.Sunk == false))
            {
                board = game.Attack(board, CoordinatesExtension.GetRow(attackPosition),
                    CoordinatesExtension.GetColumn(attackPosition));
                DisplayBoard(board);
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

                if (cell.Column == _columnsCount)
                {
                    Console.WriteLine();
                }
            }
        }

        private static void WriteColumnHeader(Board board)
        {
            var columnsCount = board.Cells.Max(c => c.Column);
            var header = " ";
            for (int i = 1; i <= columnsCount; i++)
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
                return game.PlaceShip(board, ship, randomGenerator.Next(1, _rowsCount + 1),
                    randomGenerator.Next(1, _columnsCount + 1));
            }
            catch (Exception)
            {
                return PlaceShipRandomly(board, ship, game);
            }
        }
    }
}