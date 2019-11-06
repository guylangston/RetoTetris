using System;
using System.Collections.Generic;
using System.Linq;
using VectorInt;

namespace Tetris.Lib.Math
{


    public interface IMatrix2<T> : IEquatable<IMatrix2<T>>
    {
        T this[VectorInt2 pos] { get; set; }
        T this[int x, int y] { get; set; }
        VectorInt2 Size { get; }

        IEnumerable<VectorInt2> ForEach(T match);
        IEnumerable<(VectorInt2 pos, T data)> ForAll(bool includeDefault = false);
    }


    public class Matrix2<T> : IMatrix2<T>
    {
        private readonly T[,] inner;

        public Matrix2(VectorInt2 size)
        {
            Size = size;
            inner = new T[Size.X, Size.Y];
        }

        public Matrix2(IEnumerable<VectorInt2> points, T fill)
        {
            var temp = points.ToArray();
            Size = (temp.Max(x=>x.X)+1, temp.Max(x=>x.Y)+1);
            inner = new T[Size.X, Size.Y];
            foreach (var p in temp)
            {
                inner[p.X, p.Y] = fill;
            }
        }

        // Soft get (range check yields 0)
        // Hard set
        public T this[VectorInt2 pos]
        {
            get
            {
                if (pos.X < 0 || pos.Y < 0 || pos.X >= Size.X || pos.Y >= Size.Y)
                {
                    return default(T);
                }
                return inner[pos.X, pos.Y];
            }
            set => inner[pos.X, pos.Y] = value;
        }

        public T this[int x, int y]
        {
            get => this[new VectorInt2(x, y)];
            set => this[new VectorInt2(x, y)] = value;
        }

        public VectorInt2 Size { get; }


        public static Matrix2<T> Create(IReadOnlyList<string> lines, Func<char, T> isOn)
        {
            var m = new Matrix2<T>((lines.Max(x=>x.Length), lines.Count));
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    m[x, y] = isOn(lines[y][x]);
                }
            }
            return m;
        }

        public bool Equals(IMatrix2<T> other)
        {
            if (!Size.Equals(other.Size)) return false;

            if (other is Matrix2<T> m) return Equals(inner, m.inner);

            throw new NotImplementedException();

        }

        public IEnumerable<VectorInt2> ForEach()
        {
            for(int x=0; x<Size.X; x++)
            for (int y = 0; y < Size.Y; y++)
            {
                if (inner[x,y] != null) yield return new VectorInt2(x,y);
            }
        }

        
        public IEnumerable<VectorInt2> ForEach(T match)
        {
            for(int x=0; x<Size.X; x++)
            for (int y = 0; y < Size.Y; y++)
            {
                if (match.Equals(inner[x,y])) yield return new VectorInt2(x,y);
            }
        }

        public IEnumerable<(VectorInt2 pos, T data)> ForAll(bool includeDefault = false)
        {
            for(int x=0; x<Size.X; x++)
            for (int y = 0; y < Size.Y; y++)
            {
                var d = inner[x, y];
                if (includeDefault)
                {
                    yield return (new VectorInt2(x, y), d);
                }
                else if (!object.Equals(d, default(T)))
                {
                    yield return (new VectorInt2(x, y), d);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Matrix2<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((inner != null ? inner.GetHashCode() : 0) * 397) ^ Size.GetHashCode();
            }
        }
        
        
    }
}