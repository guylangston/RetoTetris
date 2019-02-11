using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using ConsoleZ;
using ConsoleZ.Drawing;
using ConsoleZ.Win32;
using Tetris.Lib.Math;


namespace Tetris.Lib.Rendering
{
    public class TextConsoleRenderer : IRenderer2<ConsolePixel>
    {
        private ConsolePixel[,] active;
        private ConsolePixel[,] buffer;

        public TextConsoleRenderer(IntVector2 size)
        {
            Size = size;
            buffer = new ConsolePixel[size.X, size.Y];
            Fill(DefaultPixel);

            active = buffer;

            buffer = new ConsolePixel[size.X, size.Y];
            Fill(DefaultPixel);
        }

        public bool IsColourEnabled { get; set; } = true;
        public ConsolePixel DefaultPixel = new ConsolePixel(' ', Color.LightGray, Color.Black);
        public IntVector2 Size { get; }

        public int Width => Size.X;
        public int Height => Size.Y;
        

        private IBufferedAbsConsole<CHAR_INFO> screenDevice;
        public void Init()
        {
            ConsoleZ.DirectConsole.MaximizeWindow();
            ConsoleZ.DirectConsole.Setup(80, 40, 10, 10, "Consolas");
            ConsoleZ.DirectConsole.Fill(' ', 0);
            screenDevice = ConsoleZ.DirectConsole.Singleton;
        }

        private void WriteUsingConsoleZ()
        {
            var ww = System.Math.Min(screenDevice.Width, Width);
            var hh = System.Math.Min(screenDevice.Height, Height);
            for (var x = 0; x < ww; x++)
            {
                for (var y = 0; y < hh; y++)
                {
                    screenDevice[x, y] = MapToConsole(active[x, y]);
                }
            }
            screenDevice.Update();
        }

        private CHAR_INFO MapToConsole(ConsolePixel consolePixel)
        {
            byte c = (byte)ConsoleColor.White;
            if (ConsoleHelper.ToConsole.TryGetValue(consolePixel.Fore, out var f))
            {
                c = (byte) f;
            }
            if (ConsoleHelper.ToConsole.TryGetValue(consolePixel.Back, out var b))
            {
                var bb = ((int)b) << 4;
                c = (byte)(c + bb);
            }
            
            return new CHAR_INFO(consolePixel.Char,c );
        }


        public void Fill(ConsolePixel p)
        {
            for (var y = 0; y < Size.Y; y++)
                for (var x = 0; x < Size.X; x++)
                {
                    buffer[x, y] = p;
                }
        }

        public ConsolePixel this[int x, int y]
        {
            get => this[new IntVector2(x, y)];
            set => this[new IntVector2(x, y)] = value;
        }

        public ConsolePixel this[IntVector2 p]
        {
            get
            {
                if (p.X < 0 || p.Y < 0 || p.X >= Size.X || p.Y >= Size.Y)
                {
                    return default;
                }
                return buffer[p.X, p.Y];
            }
            set
            { 
                if (p.X < 0 || p.Y < 0 || p.X >= Size.X || p.Y >= Size.Y)
                {
                    return;
                }
                buffer[p.X, p.Y] = value;
            }
        }

        public ConsolePixel this[float x, float y]
        {
            get => this[new IntVector2((int) x, (int) y)];
            set => this[new IntVector2((int) x, (int) y)] = value;
        }

        public void DrawText(IntVector2 p, string txt, ConsolePixel style)
        {
            if (txt == null) return;
            for (int i = 0; i < txt.Length; i++)
            {
                this[p.AddX(i)] = new ConsolePixel(txt[i], style.Fore, style.Back);
            }
        }


        public void DrawLine(IntVector2 tl, IntVector2 br, ConsolePixel pixel) => this.DrawLine(tl.X, tl.Y, br.X, br.Y, pixel);


        public void DrawText(int x, int y, string txt, ConsolePixel style)
        {
            for (int c = 0; c < txt.Length; c++)
            {
                if (x + c > Width)
                {
                    x = 0;
                    y++;
                    if (y > Height) return;
                }
                this[x + c, y] = new  ConsolePixel(txt[c], style.Fore, style.Back);
            }
        }

        public void DrawSprite(int x, int y, string[] sprite, Color fg, Color bg)
        {
            for(var a = 0; a< sprite.Length; a++)
            for(var b = 0; b< sprite[a].Length; b++)
                this[x+b, y+a] = new ConsolePixel(sprite[a][b], fg, bg);
            
        }

        public virtual void Update()
        {
            // swap
            var t = active;
            active = buffer;
            buffer = t;

            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            if (IsColourEnabled)
            {
                WriteUsingConsoleZ();
                //  WriteDiffs();
                //GotoAndWriteSingle();
            }
            else
            {
                WriteFullLinesNoColour();
            }

           
        }

        
        Dictionary<Color, ConsoleColor> Palette = new Dictionary<Color, ConsoleColor>()
        {
            {Color.Black, ConsoleColor.Black},
            {Color.Blue, ConsoleColor.Blue},
            {Color.Yellow, ConsoleColor.Yellow},
            {Color.Red, ConsoleColor.Red},
            {Color.Cyan, ConsoleColor.Cyan},
            {Color.Green, ConsoleColor.Green},
            {Color.Purple, ConsoleColor.Magenta},
            {Color.Orange, ConsoleColor.DarkRed},
            {Color.Gray, ConsoleColor.Gray},
        };
        private void WriteDiffs()
        {

            for (var y = 0; y < Size.Y - 1; y++)
            {
                for (var x = 0; x < Size.X; x++)
                {
                    var a = active[x, y];
                    var b = buffer[x, y];
                    if (!a.Equals(b))
                    {
                        if (Palette.TryGetValue(b.Fore, out var cc))
                        {
                            Console.ForegroundColor = cc;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        if (Palette.TryGetValue(b.Back, out var bk))
                        {
                            Console.BackgroundColor = bk;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }

                        Console.SetCursorPosition(x, y);
                        Console.Write(b.Char);
                    }
                }
            }
            Console.SetCursorPosition(Size.X-2, Size.Y-1);


        }

        private void WriteFullLinesNoColour()
        {
            var sb = new StringBuilder(Size.X);
            System.Console.Clear();
            for (var y = 0; y < Size.Y - 1; y++)
            {
                sb.Clear();
                for (var x = 0; x < Size.X; x++)
                {
                    sb.Append(buffer[x, y].Char);
                }

                System.Console.Write(sb);
            }
        }

        private void GotoAndWriteSingle()
        {
            System.Console.Clear();

            for (var y = 0; y < Size.Y - 1; y++)
            {
                for (var x = 0; x < Size.X; x++)
                {
                    var b = this[x, y];
                    if (b.Char != ' ')
                    {
                        if (Palette.TryGetValue(b.Fore, out var cc))
                        {
                            Console.ForegroundColor = cc;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }

                        Console.SetCursorPosition(x, y);
                        Console.Write(b.Char);
                    }
                }
            }
        }
    }
}