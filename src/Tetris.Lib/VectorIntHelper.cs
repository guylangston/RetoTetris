using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorInt;

namespace Tetris.Lib
{
    public static class VectorIntHelper
    {
        public static VectorInt2 AddX(this VectorInt2 v, int xx) => v + (xx, 0);
        public static VectorInt2 AddY(this VectorInt2 v, int yy) => v + (0, yy);
    }
}
