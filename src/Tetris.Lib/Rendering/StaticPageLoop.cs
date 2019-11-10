using System;
using System.Drawing;
using ConsoleZ.Drawing;
using ConsoleZ.Drawing.Game;

namespace Tetris.Lib.Rendering
{
    public class StaticPageLoop : GameScene<MasterGameLoop, ConsolePixel>
    {
        public IGameLoop Next { get; set; }
        private IRenderer<ConsolePixel> renderer => Parent.Renderer;
        private MasterGameLoop master;
        private readonly string bgfile;
        private string[] bg;

        public StaticPageLoop(MasterGameLoop master, string bgfile) : base(master)
        {
            this.master = master;
            this.bgfile = bgfile;
        }

        public override void Init()
        {
            this.bg = System.IO.File.ReadAllLines(master.ResourceManager.GetResource(bgfile));
        }

        public override void Reset()
        {
            activeTime = 0;
        }

        public override void Step(float elapsedSec)
        {
            activeTime += elapsedSec;

            if (Console.KeyAvailable)
            {
                var k = Console.ReadKey();
                if (k.KeyChar == '1')
                {
                    master.Active = master.Tetris;
                }
                if (k.KeyChar == '2')
                {
                    master.Active = master.Tetris;
                }

                if (k.Key == ConsoleKey.Spacebar)
                {
                    master.Active = Next;
                }
            }

            if (activeTime > 10)
            {
                if (Next != null)
                {
                    master.Active = Next;
                }
            }
        }

        private float activeTime = 0;
        

        public override void Draw()
        {
            renderer.Fill(renderer.DefaultPixel());

            renderer.DrawSprite(0, 0, bg, Color.Gray, Color.Black);

            renderer.DrawText(1,1, $"{activeTime:0.0} sec", new ConsolePixel(' ', Color.Yellow, Color.Black));

            renderer.Update();
        }

        public override void Dispose()
        {
            
        }
    }
}