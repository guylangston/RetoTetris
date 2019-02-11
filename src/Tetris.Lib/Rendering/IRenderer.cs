using System.Drawing;
using ConsoleZ.Drawing;
using Tetris.Lib.Math;

namespace Tetris.Lib.Rendering
{
   

    public interface IRenderer2<TPixel> : IRenderer<TPixel>
    {
        IntVector2 Size { get; }

        TPixel this[IntVector2 p] { get; set; } // Get/Set Pixel
    }

    

    

    
}