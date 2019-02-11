using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tetris.Lib.Util
{
    public class RandomHelper
    {
        public static readonly Random Random = new Random((int)DateTime.Now.Ticks); 
        
        public static T ChooseOne<T>(IReadOnlyList<T> list)
        {
            if (list == null || !list.Any()) return default(T);
            return list[Random.Next(0, list.Count - 1)];
        }
    }
}