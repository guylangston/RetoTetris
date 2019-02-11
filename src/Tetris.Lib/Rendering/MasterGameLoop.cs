namespace Tetris.Lib.Rendering
{
    public class MasterGameLoop : GameLoopBase
    {
        public TetrisGameLoop Tetris { get; private set; }
        public StaticPageLoop Intro { get; private set; }
        public StaticPageLoop Scores { get; private set; }
        public StaticPageLoop History { get; private set; }

        public ResourceManager ResourceManager { get; } = new ResourceManager();

        private TextConsoleRenderer renderer;

        private IGameLoop active;

        public IGameLoop Active
        {
            get { return active; }
            set
            {
                active = value;
                if (value is GameLoopBase sp) sp.Reset();
                if (value is GameLoopProxy p) p.Reset();
            }
        }

        public MasterGameLoop(TextConsoleRenderer renderer)
        {
            this.renderer = renderer;
        }

        public override void Init()
        {
            Tetris = new TetrisGameLoop(renderer, this.ResourceManager, this);
            Tetris.Init();

            Intro = new StaticPageLoop(renderer, this, "intro.txt");
            Intro.Init();

            History = new StaticPageLoop(renderer, this, "history.txt");
            History.Init();

            Scores = new StaticPageLoop(renderer, this, "hiscore.txt");
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