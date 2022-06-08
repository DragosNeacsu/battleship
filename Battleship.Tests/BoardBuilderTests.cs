using System;
using Battleship.Core;
using Battleship.Core.Models;
using FluentAssertions;
using Xunit;

namespace Battleship.Tests;

public class BoardBuilderTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void ShouldThrowErrorWhenInvalidParameters(int rows, int columns)
    {
        //When
        Action act = () => BoardBuilder.Create(rows, columns);

        //Then
        act.Should().Throw<ArgumentException>().WithMessage("Invalid board size");
    }
    
    [Theory]
    [InlineData(6, 6)]
    [InlineData(10, 10)]
    public void ShouldReturnValidBoardWhenCreatingOne(int rows, int columns)
    {
        //When
        var board = BoardBuilder.Create(rows, columns);
    
        //Then
        board.Cells.Count.Should().Be(rows * columns);
    }
    
    [Theory]
    [InlineData(6, 6)]
    [InlineData(10, 10)]
    public void ShouldMarkEveryCellAsWater(int rows, int columns)
    {
        //When
        var board = BoardBuilder.Create(rows, columns);
    
        //Then
        foreach (var cell in board.Cells)
        {
            cell.Status.Should().Be(CellStatus.Water);
        }
    }
}