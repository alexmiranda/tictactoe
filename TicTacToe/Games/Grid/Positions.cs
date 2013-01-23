using System;
using System.Reflection;

namespace TicTacToe.Games.Grid
{
    public static class Positions
    {
        internal static T GetByName<T>(string name) where T : IPosition
        {
            var field = typeof (T).GetField(name, BindingFlags.Public | BindingFlags.Static);
            if (field == null)
                throw new ArgumentException("Position does not exist");
            return (T) field.GetValue(null);
        }
    }
}