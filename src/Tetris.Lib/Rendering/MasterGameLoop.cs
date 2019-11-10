using System;
using ConsoleZ.Drawing;
using ConsoleZ.Drawing.Game;

namespace Tetris.Lib.Rendering
{
    public class MasterGameLoop : GameScene<IRenderingGameLoop<ConsolePixel>, ConsolePixel>
    {
        private IGameLoop active;

        public MasterGameLoop(IRenderingGameLoop<ConsolePixel> parent) : base(parent)
        {
        }

        public TetrisGameLoop  Tetris          { get; private set; }
        public StaticPageLoop  Intro           { get; private set; }
        public StaticPageLoop  Scores          { get; private set; }
        public StaticPageLoop  History         { get; private set; }
        public ResourceManager ResourceManager { get; } = new ResourceManager();

        public IGameLoop Active
        {
            get => active;
            set
            {
                active = value;
                active.Reset();
            }
        }

        public override void Init()
        {
            Tetris = new TetrisGameLoop(this.ResourceManager, this);
            Tetris.Init();

            Intro = new StaticPageLoop(this, "intro.txt");
            Intro.Init();

            History = new StaticPageLoop( this, "history.txt");
            History.Init();

            Scores = new StaticPageLoop( this, "hiscore.txt");
            Scores.Init();

            // Loop
            Intro.Next = History;
            History.Next = Scores;
            Scores.Next = Intro;

            // Start with intro
            Active = Intro;
        }

        public override void Step(float elapsedSec)
        {
            if (active != Tetris)
            {
                if (Input.IsKeyPressed(ConsoleKey.D1))
                {
                    Active = Tetris;
                }
                
                if (Input.IsKeyPressed(ConsoleKey.Q))
                {
                    Environment.Exit(0);
                }
            }
            
            active.Step(elapsedSec);
        }

        public override void Draw()
        {
            active.Draw();
        }

        public override void Dispose()
        {
            
        }
    }
}