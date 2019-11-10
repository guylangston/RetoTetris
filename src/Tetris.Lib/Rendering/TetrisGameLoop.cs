using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using ConsoleZ.Drawing;
using ConsoleZ.Drawing.Game;
using Newtonsoft.Json;
using Tetris.Lib.Logic;
using Tetris.Lib.Math;
using VectorInt;


namespace Tetris.Lib.Rendering
{
    
    public class TetrisGameLoop : GameScene<MasterGameLoop, ConsolePixel>
    {
        public TetrisGameLoop( ResourceManager resourceManager, MasterGameLoop master ) : base(master)
        {
            
            this.resourceManager = resourceManager;
            this.gameA = new TetrisGame();
            this.gameB = new TetrisGame();

            
            //KeyA = KeyXArcadeLeft;
            //KeyB = KeyXArcadeRight;
            KeyA = KeyArrows;
            KeyB = KeyWASD;

        
        }



        private IRenderer<ConsolePixel> render => Parent.Renderer;
        private readonly ResourceManager resourceManager;
        private TetrisGame gameA;
        private TetrisGame gameB;
        private int frame;
        private string[] bg;

        public TetrisGame GameA => gameA;
        public TetrisGame GameB => gameB;
        
        Bouncer[] b = new Bouncer[]
        {
            new Bouncer()
            {
                Position =  new Vector2(40, 7),
                Speed = new Vector2(2f, 1.5f)
            },
            new Bouncer()
            {
                Position =  new Vector2(70, 30),
                Speed = new Vector2(-1f, 1f),
                Pixel = new ConsolePixel('@', Color.White, Color.Green)
            }
        } ;

        

        public override void Init()
        {
            this.bg = System.IO.File.ReadAllLines(resourceManager.GetResource(@"background.txt"));

        }

        public override void Reset()
        {
            base.Reset();
            gameA.SendInput(Logic.Input.Start);
            gameB.SendInput(Logic.Input.Start);
        }

        public class KeyMap
        {
            public ConsoleKey Left { get; set; }
            public ConsoleKey Right { get; set; }
            public ConsoleKey Rotate { get; set; }
            public ConsoleKey Drop { get; set; }
        }

        public static readonly KeyMap KeyXArcadeLeft = new KeyMap()
        {
            Left = ConsoleKey.LeftArrow,
            Right = ConsoleKey.RightArrow,
            Rotate = ConsoleKey.C,
            Drop = ConsoleKey.DownArrow
            //Drop = ConsoleKey.D5
        };

        public static readonly KeyMap KeyXArcadeRight  = new KeyMap()
        {
            Left = ConsoleKey.D,
            Right = ConsoleKey.G,
            Rotate = ConsoleKey.Oem6,
            Drop = ConsoleKey.F
            //Drop = ConsoleKey.D6
        };

        public static readonly KeyMap KeyWASD  = new KeyMap()
        {
            Left = ConsoleKey.A,
            Right = ConsoleKey.D,
            Rotate = ConsoleKey.W,
            Drop = ConsoleKey.S
        };
        public static readonly KeyMap KeyArrows = new KeyMap()
        {
            Left = ConsoleKey.LeftArrow,
            Right = ConsoleKey.RightArrow,
            Rotate = ConsoleKey.UpArrow,
            Drop = ConsoleKey.DownArrow
        };

        public KeyMap KeyA { get; set; }
        public KeyMap KeyB { get; set; }


        private float accSec = 0;

        public override void Step(float elapsedSec)
        {
            accSec += elapsedSec;

            foreach (var bb in b)
            {
                bb.Step(render, elapsedSec);    
            }

            if (Input.IsKeyPressed())
            {
//                
//                if ((kk.Modifiers & ConsoleModifiers.Control) > 0 && kk.Key == ConsoleKey.S)
//                {
//                    File.WriteAllText(Path.Combine(resourceManager.GetResource("./saves/"), $"Tetris-GameA-{DateTime.Now.Ticks}.json"), JsonConvert.SerializeObject(gameA.CaptureState(), Formatting.Indented));
//                    File.WriteAllText(Path.Combine(resourceManager.GetResource("./saves/"), $"Tetris-GameB-{DateTime.Now.Ticks}.json"), JsonConvert.SerializeObject(gameB.CaptureState(), Formatting.Indented));
//                }
//                if ((kk.Modifiers & ConsoleModifiers.Control) > 0 && kk.Key == ConsoleKey.L)
//                {
//                    gameA.Load(JsonConvert.DeserializeObject<TetrisGameStateDto>(File.ReadAllText(resourceManager.GetResource("game-default.json"))));
//                    
//                }

                if (Input.IsKeyPressed(KeyA.Left))     gameA.SendInput(Logic.Input.MoveLeft);
                if (Input.IsKeyPressed(KeyA.Right))    gameA.SendInput(Logic.Input.MoveRight);
                if (Input.IsKeyPressed(KeyA.Drop))     gameA.SendInput(Logic.Input.Drop);
                if (Input.IsKeyPressed(KeyA.Rotate))   gameA.SendInput(Logic.Input.Rotate);

                if (Input.IsKeyPressed(KeyB.Left))     gameB.SendInput(Logic.Input.MoveLeft);
                if (Input.IsKeyPressed(KeyB.Right))    gameB.SendInput(Logic.Input.MoveRight);
                if (Input.IsKeyPressed(KeyB.Drop))     gameB.SendInput(Logic.Input.Drop);
                if (Input.IsKeyPressed(KeyB.Rotate))   gameB.SendInput(Logic.Input.Rotate);

                
                if (Input.IsKeyPressed(ConsoleKey.Escape))
                {
                    gameA.SendInput(Logic.Input.Quit);
                    gameB.SendInput(Logic.Input.Quit);
                    Parent.Active = Parent.Scores;
                }
                
            }

            // One step per second
            if (accSec >= 1) // Crude 1 per sec; TODO: accumulate elapsedSec until > 1
            {
                accSec = 0;
                gameA.Step();    
                gameB.Step();
            }
            
        }
        
        public override void Draw()
        {
            render.Fill(render.DefaultPixel());

            render.DrawSprite(0, 0, bg, Color.Gray, Color.Black);

            Draw(gameA, (20,7), (3,3));

            Draw(gameB, (56,7), (45,3));
            
            // Particles
            foreach (var bb in b)
            {
                render[new VectorInt2(bb.Position)] = bb.Pixel;
            }
            
            render.Update();

            Console.Title = $"FPS: {FramesPerSecond:0.0}/sec, {gameA.State} -- {FrameCount}";
        }

        public  void Draw(TetrisGame game, VectorInt2 surface, VectorInt2 scores)
        {
            
            var styleText= new ConsolePixel(' ', Color.Yellow, Color.Black);

            // Score
            render.DrawText(scores + (0, 0), game.Score.ToString().PadLeft(5), styleText);

            // Lines
            render.DrawText(scores + (0, 3), game.Lines.ToString().PadLeft(5), styleText);

            // Pieces
            render.DrawText(scores + (0, 6), game.PieceCount.ToString().PadLeft(5), styleText);

            // Timer
            //render.DrawText(scores + (0, 9), Elapsed.TotalSeconds.ToString("0.00").PadLeft(5), styleText);

            if (game.Active != null)
            {
                Draw(game.Active, surface);
            }
            

            if (game.Floor != null)
            {
                foreach(var p in game.Floor.ForAll())
                {
                    render[surface + p.pos] = p.data.Style;
                }
            }

            if (game.LineCompleted >= 0)
            {
                render.DrawText(
                    surface.AddY(game.LineCompleted),
                     "-=##=--=##=--=##=--=##=--=##=--=##=--=##=--=##=--=##=--=##=-"
                         .Substring(base.FrameCount/10 % 10, game.Width),
                    new ConsolePixel('=', Color.Black, Color.DarkBlue) 
                    );

                //render.DrawLine(
                    //surface.AddY(game.LineCompleted),
                    //surface + (game.Width, game.LineCompleted), 
                    //new ConsolePixel('=', Color.Red, Color.DarkRed) );
            }

            if (game.Next != null)
            {
                // Next
                foreach (var b in game.Next.Bricks)
                {
                    render[scores + (0, 13) + b ] = game.Next.Style;
                }
            }
            

            
            

        }

       

        public override void Dispose()
        {
            Console.ResetColor();
        }

        private void Draw(Piece p, VectorInt2 loc)
        {
            foreach (var b in p.GetBricks())
            {
                render[loc + b] = p.Tetromino.Style;
            }
        }

    }
}