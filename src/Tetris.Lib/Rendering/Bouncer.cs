using System.Drawing;
using System.Numerics;
using ConsoleZ.Drawing;
using Tetris.Lib.Math;
using VectorInt;

namespace Tetris.Lib.Rendering
{
    public class Bouncer
    {
        public ConsolePixel Pixel { get; set;}= new ConsolePixel('*', Color.Red, Color.Black);

        public Vector2  Position { get; set; } // in 1/10ths
        public Vector2 Speed { get; set; } // in 1/10ths


        public void Step(IRenderer<ConsolePixel> render, float time)
        {
            var p = new VectorInt2(Position);
            var next = Position + Speed*time;
            var i = new VectorInt2(next);

            if (i == p)
            {
                Position = next;
                return;
            }

            if (i.X != p.X)
            {
                if (i.X < 0 || i.X > render.Width || render[i].Char != ' ')
                {
                    Speed = Speed * new Vector2(-1f, 1f);
                }
            }

            
            if (i.Y != p.Y)
            {
                if (i.Y < 0 || i.Y > render.Height || render[i].Char != ' ')
                {
                    Speed = Speed * new Vector2(1f, -1f);
                }
            }
           
            Position = Position + Speed*time;
            
        }

        private bool Collision(IRenderer<ConsolePixel> render, Vector2 next)
        {
            if (next.X < 0 || next.X > render.Width) return true;
            if (next.Y < 0 || next.Y > render.Height) return true;

            var p = new VectorInt2(Position);
            var i = new VectorInt2(next);

            if (p == i) return false;

            if (render[i].Char != ' ')
            {
                return true;
            }

            return false;
        }
    }
}