using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Tetris.Lib.Math
{
  
    /// <summary>
    /// <see cref="System.Numerics.Vector{T}"/>
    /// </summary>
    public struct IntVector2  : IEquatable<IntVector2>,  IEquatable<ValueTuple<int, int>>, IEquatable<Tuple<int, int>>
    {
        public IntVector2(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public IntVector2(Vector2 v) : this()
        {
            X = (int)v.X;
            Y = (int)v.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public IntVector2 AddX(int x) => new IntVector2(X +x, Y);
        public IntVector2 AddY(int y) => new IntVector2(X, Y+y);

        public IntVector2 SetX(int x) => new IntVector2(x, Y);
        public IntVector2 SetY(int y) => new IntVector2(X, y);


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj is IntVector2 v) return Equals(v);
            if (obj is Tuple<int, int> tp) return X==tp.Item1 && Y==tp.Item2;
            if (obj is ValueTuple<int, int> vt) return X==vt.Item1 && Y==vt.Item2;
            return false;
            
        }
        public bool Equals(IntVector2 other) => X == other.X && Y == other.Y;
        public bool Equals(ValueTuple<int, int> other) => X == other.Item1 && Y == other.Item2;
        public bool Equals(Tuple<int, int> other) => X == other.Item1 && Y == other.Item2;

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(IntVector2 lhs, IntVector2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
        public static bool operator !=(IntVector2 lhs, IntVector2 rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;

        public static IntVector2 operator +(IntVector2 lhs, IntVector2 rhs) => new IntVector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        public static IntVector2 operator -(IntVector2 lhs, IntVector2 rhs) => new IntVector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        public static IntVector2 operator *(IntVector2 lhs, IntVector2 rhs) => new IntVector2(lhs.X * rhs.X, lhs.Y * rhs.Y);
        public static IntVector2 operator /(IntVector2 lhs, IntVector2 rhs) => new IntVector2(lhs.X / rhs.X, lhs.Y / rhs.Y);

        public static  implicit operator IntVector2((int, int) tpl) => new IntVector2(tpl.Item1, tpl.Item2);
        public static implicit operator (int x, int y)(IntVector2 v2) => (v2.X, v2.Y);

        public Vector2 ToVector2() => new Vector2(X, Y);

        public override string ToString() => $"({X}, {Y})";

        
    }

   
}