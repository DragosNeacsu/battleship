using System;
using System.Linq;
using Battleship.Core;
using Battleship.Core.Models;
using FluentAssertions;
using Xunit;

namespace Battleship.Tests;

public class GameTests
{
    [Theory]
    [InlineData(1, 20)]
    [InlineData(20, 1)]
    public void ShouldThrowWhenTryingToAddShipOutsideTheBoard(int row, int column)
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var ship = ShipBuilder.Build(ShipType.Battleship);

        //When
        Action act = () => Game.PlaceShip(board, ship, row, column);

        //Then
        act.Should().Throw<IndexOutOfRangeException>().WithMessage("Position outside the board");
    }

    [Theory]
    [InlineData(ShipType.Battleship, 1, 8)]
    [InlineData(ShipType.Destroyer, 9, 7)]
    public void ShouldThrowWhenTryingToAddShipOnTheBoardEdge(ShipType shipType, int row, int column)
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var ship = ShipBuilder.Build(shipType);

        //When
        Action act = () => Game.PlaceShip(board, ship, row, column);

        //Then
        act.Should().Throw<IndexOutOfRangeException>().WithMessage($"Ship will not fit at {row},{column}");
    }

    [Theory]
    [InlineData(ShipType.Battleship, 1, 3)]
    [InlineData(ShipType.Destroyer, 2, 1)]
    public void ShouldPlaceOneShipOnTheBoard(ShipType shipType, int row, int column)
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var ship = ShipBuilder.Build(shipType);

        //When
        board = Game.PlaceShip(board, ship, row, column);

        //Then
        ship.Column.Should().Be(column);
        ship.Row.Should().Be(row);
        var shipCells =
            board.Cells.Where(c => c.Row == row && Enumerable.Range(column, ship.Size).Any(x => x == c.Column));
        foreach (var shipCell in shipCells)
        {
            shipCell.Status.Should().Be(CellStatus.Ship);
        }
    }

    [Fact]
    public void ShouldPlaceMultipleShipsOnTheBoard()
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var battleShip1 = ShipBuilder.Build(ShipType.Battleship);
        var battleShip2 = ShipBuilder.Build(ShipType.Battleship);
        var destroyer1 = ShipBuilder.Build(ShipType.Destroyer);

        //When
        board = Game.PlaceShip(board, battleShip1, 1, 1);
        board = Game.PlaceShip(board, battleShip2, 2, 1);
        board = Game.PlaceShip(board, destroyer1, 3, 1);

        //Then
        board.Cells.Where(c => c.Row == 1 && Enumerable.Range(1, battleShip1.Size).Any(x => x == c.Column)).ToList()
            .ForEach(x => x.Status.Should().Be(CellStatus.Ship));
        board.Cells.Where(c => c.Row == 2 && Enumerable.Range(1, battleShip2.Size).Any(x => x == c.Column)).ToList()
            .ForEach(x => x.Status.Should().Be(CellStatus.Ship));
        board.Cells.Where(c => c.Row == 3 && Enumerable.Range(1, destroyer1.Size).Any(x => x == c.Column)).ToList()
            .ForEach(x => x.Status.Should().Be(CellStatus.Ship));
    }

    [Fact]
    public void ShouldThrowWhenTryingToAddShipOnAnOccupiedCell()
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var battleShip1 = ShipBuilder.Build(ShipType.Battleship);
        var battleShip2 = ShipBuilder.Build(ShipType.Battleship);

        //When
        Action act = () =>
        {
            board = Game.PlaceShip(board, battleShip1, 1, 1);
            board = Game.PlaceShip(board, battleShip2, 1, 3);
        };

        //Then
        act.Should().Throw<Exception>().WithMessage("Can't place ship at this location");
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(-1, 20)]
    public void ShouldValidateTheAttack(int row, int column)
    {
        //Given
        var board = BoardBuilder.Create(10, 10);

        //When
        Action act = () => board = Game.Attack(board, row, column);

        //Then
        act.Should().Throw<IndexOutOfRangeException>().WithMessage("Position outside the board");
    }


    [Fact]
    public void ShouldMarkAnAttackWithHitIfThereWasAShip()
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var battleShip = ShipBuilder.Build(ShipType.Battleship);

        //When
        board = Game.PlaceShip(board, battleShip, 1, 1);
        board = Game.Attack(board, 1, 2);

        //Then
        board.Cells.First(c => c.Row == 1 && c.Column == 2).Status.Should().Be(CellStatus.Hit);
    }

    [Fact]
    public void ShouldMarkAnAttackWithMissIfThereWasWater()
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var battleShip = ShipBuilder.Build(ShipType.Battleship);

        //When
        board = Game.PlaceShip(board, battleShip, 1, 1);
        board = Game.Attack(board, 2, 2);

        //Then
        board.Cells.First(c => c.Row == 2 && c.Column == 2).Status.Should().Be(CellStatus.Miss);
    }

    [Fact]
    public void ShouldMarkWithSunkIfTheShipWasDestroyed()
    {
        //Given
        var board = BoardBuilder.Create(10, 10);
        var battleShip = ShipBuilder.Build(ShipType.Battleship);

        //When
        board = Game.PlaceShip(board, battleShip, 1, 2);
        board = Game.Attack(board, 1, 1);
        board = Game.Attack(board, 1, 2);
        board = Game.Attack(board, 1, 3);
        board = Game.Attack(board, 2, 3);
        board = Game.Attack(board, 1, 4);
        board = Game.Attack(board, 1, 5);
        board = Game.Attack(board, 1, 6);
        board = Game.Attack(board, 1, 7);

        //Then
        board.Cells.Where(c => c.Row == 1 && Enumerable.Range(2, battleShip.Size).Any(x => x == c.Column)).ToList()
            .ForEach(x => x.Status.Should().Be(CellStatus.Sink));
        board.Cells.First(c => c.Row == 2 && c.Column == 3).Status.Should().Be(CellStatus.Miss);
        board.Cells.First(c => c.Row == 1 && c.Column == 1).Status.Should().Be(CellStatus.Miss);
        board.Cells.First(c => c.Row == 1 && c.Column == 7).Status.Should().Be(CellStatus.Miss);
        board.Ships.First().Sunk.Should().BeTrue();
    }
}