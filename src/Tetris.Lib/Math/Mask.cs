using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Lib.Math
{

    
    public interface IMask : IMatrix2<bool>, IEnumerable<IntVector2>
    {

    }

    public class Mask : Matrix2<bool>, IMask
    {
        public Mask(IntVector2 size) : base(size)
        {
        }

        public Mask(IEnumerable<IntVector2> points) : base(points, true)
        {
        }

        public IEnumerator<IntVector2> GetEnumerator() => base.ForEach(true).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Mask Create(IReadOnlyList<string> lines)
        {
            var ret = new Mask((lines.Max(x => x.Length), lines.Count));
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    ret[x,y] = IsTrue(lines[y][x]);
                }
            }
            return ret;
        }

        public static bool IsTrue(char c) => c != ' ' && c != '.';
    }

    public static class MaskHelper
    {
        public static IMask RotateRight(this IMask matrix2, int r)
        {
            if (r < 0) r = (4 + (r % 4));
            switch (r % 4)
            {
                case 0 : return matrix2;
                case 1 : return new Mask(matrix2.Select(p=>new IntVector2(matrix2.Size.Y - p.Y -1, p.X )));
                case 2 : return new Mask(matrix2.Select(p=>new IntVector2(matrix2.Size.X - p.X -1,  matrix2.Size.Y - p.Y -1 )));
                case 3 : return new Mask(matrix2.Select(p=>new IntVector2(p.Y, matrix2.Size.X - p.X -1)));
                default: throw new Exception();
            }
        }

        public static IMask RotateLeft(this IMask matrix2, int r) => RotateRight(matrix2, -r);
        public static string ToMultiLineString(this IMask matrix2, char on = '#', char off = '.') => ToString(matrix2, on, off, '\n');

        public static string ToString(this IMask matrix2, char on, char off, char eol)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < matrix2.Size.Y; y++)
            {
                for (int x = 0; x < matrix2.Size.X; x++)
                {
                    sb.Append(matrix2[x, y] ? on : off);
                }
                sb.Append(eol); 
            }
            return sb.ToString();
        }

        public static string ToSingleString(this IMask matrix2) => ToString(matrix2, '#', '.', '|');
        
    }
}