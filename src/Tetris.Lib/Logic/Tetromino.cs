using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ConsoleZ.Drawing;
using Tetris.Lib.Math;
using Tetris.Lib.Rendering;
using Tetris.Lib.Util;

namespace Tetris.Lib.Logic
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Tetromino
    /// </summary>
    public class Tetromino
    {
        private Tetromino(char name, char draw, char uni, Color colour, Color bg, IMask bricks)
        {
            Name = name;
            UniDraw = uni;
            Draw = draw;
            Colour = colour;
            Background = bg;
            Bricks = bricks;
            Style = new ConsolePixel(draw, colour, bg);
        }

        

        // https://www.ascii-code.com/
        public char Name { get; }
        public char UniDraw { get; }
        public char Draw { get; }
        public Color Colour { get; }
        public Color Background { get; set; }
        public IMask Bricks { get; }
        public int Width => Bricks.Size.X;
        public int Height => Bricks.Size.Y;
        public ConsolePixel Style { get; }

        

        public static readonly Tetromino O = new Tetromino('O', '#', '█', Color.Cyan,  Color.DarkCyan, Mask.Create(new []
            {
                "XX",
                "XX"
            })
        );
        
        public static readonly Tetromino I = new Tetromino('I','#', '▒', Color.Blue, Color.DarkBlue, Mask.Create(
            new [] {"XXXX"})
        );
        
        public static readonly Tetromino T = new Tetromino('T', 'X', '░', Color.Magenta, Color.DarkMagenta, Mask.Create(new []
            {
                "XXX",
                ".X."
            })
        );
        
        public static readonly Tetromino J = new Tetromino('J', '*', '▓', Color.Yellow, Color.DarkOrange, Mask.Create(new []
            {
                "XXX",
                "..X"
            })
        );

        public static readonly Tetromino L = new Tetromino('L', '*', '▓',Color.Green, Color.DarkGreen, Mask.Create(new []
            {
                "XXX",
                "X.."
            })
        );
        
        public static readonly Tetromino S = new Tetromino('S', 'X', '#', Color.DarkRed, Color.Red,Mask.Create(new []
            {
                ".XX",
                "XX."
            })
        );
        public static readonly Tetromino Z = new Tetromino('Z', 'X', '@', Color.Red, Color.DarkRed, Mask.Create(new []
            {
                "XX.",
                ".XX"
            })
        );

        public static IReadOnlyList<Tetromino> All = new[] {O, I, T, J, L, S, Z};

        public static  Tetromino GetByChar(char c) => c == '.' ? null : All.First(x => x.Name == c);
    }
}