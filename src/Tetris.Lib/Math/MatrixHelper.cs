using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Lib.Math
{
    public static class MatrixHelper
    {
        public static IEnumerable<string> ToString<T>(this IMatrix2<T> m, Func<T, char> toChar)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < m.Size.Y; y++)
            {
                sb.Clear();
                for (var x = 0; x < m.Size.X; x++)
                {
                    sb.Append(toChar(m[x, y]));
                }

                yield return sb.ToString();
            }
        }
    }
}