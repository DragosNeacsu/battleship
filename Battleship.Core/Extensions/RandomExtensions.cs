namespace Battleship.Core.Extensions;

public static class RandomExtensions
{   
    public static T RandomEnum<T>()
    { 
        T[] values = (T[]) Enum.GetValues(typeof(T));
        return values[new Random().Next(0,values.Length)];
    }
}