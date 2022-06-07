using System.Text.RegularExpressions;

namespace Battleship.Core.Extensions;

public static class CoordinatesExtension
{
    public static int GetRow(string? attackPosition)
    {
        var result = Regex.Match(attackPosition, @"(\d+)").Value;
        int.TryParse(result, out int row);
        return row;
    }

    public static int GetColumn(string? attackPosition)
    {
        var result = Regex.Match(attackPosition, @"(\D+)").Value;
        return char.ToUpper(result.ToCharArray()[0]) - 64;
    }
}