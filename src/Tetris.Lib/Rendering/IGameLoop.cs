using System;
using System.IO;
using System.Threading;
using Tetris.Lib.Logic;
using Tetris.Lib.Math;

namespace Tetris.Lib.Rendering
{
    public class ResourceManager
    {
        private string dir;

        public ResourceManager()
        {

            
            var source = @"C:\Projects\EulerProject\NonEuler\Tetris\src\Tetris.Console\res";
            if (Directory.Exists( source))
            {
                dir = source;
                return;
            }

            if (Directory.GetCurrentDirectory().ToLowerInvariant().Trim('\\').EndsWith("res"))
            {
                dir = "./";
                return;
            }

            var sub = Path.Combine(Directory.GetCurrentDirectory(), "res");
            if (Directory.Exists(sub))
            {
                dir = sub;
                return;
            }


            throw new Exception("Cannot find asserts");
        }


        public string GetResource(string relfile) => Path.Combine(dir, relfile);
    }
    
    public interface IGameLoop
    {
        bool IsActive { get; }
        void Init();
        void Step(float elapsed);
        void Draw();
    }

    public abstract class GameLoopBase : IGameLoop, IDisposable
    {
        public bool IsActive{ get; protected set; }

        public int FrameCount { get; private set; }

        

        public DateTime EndFrame { get; private set; }
        public DateTime StartFrame { get; private set; }

        public DateTime GameStarted { get; private set; }
        public TimeSpan Elapsed => DateTime.Now - GameStarted;

        public string Message { get; set; }
        
        public float MinInterval { get; set; } = 1/60f;

        public float FramesPerSecond => (float) FrameCount / (float) Elapsed.TotalSeconds;

        public abstract void Init();

        public virtual void Reset()
        {

        }

        public void Start()
        {
          
            GameStarted = DateTime.Now;

            IsActive = true;
            while (IsActive)
            {
                StartFrame = DateTime.Now;
                Draw();
                EndFrame = DateTime.Now;
                var elapse = (float)(EndFrame - StartFrame).TotalSeconds;
                if (elapse < MinInterval)
                {
                    Thread.Sleep((int)((MinInterval - elapse)*1000f));
                    elapse = MinInterval;
                }

                Step(elapse);

                FrameCount++;
            }

            Message = "Bye Bye";
            // Dispose should be called next
        }
        

        public abstract void Step(float elapsedSec);

        public abstract void Draw();
        
        public abstract void Dispose();
    }

    public abstract class GameLoopProxy : IGameLoop, IDisposable
    {
        protected GameLoopProxy(GameLoopBase parent)
        {
            Parent = parent;
        }

        protected GameLoopBase Parent { get;  }

        public bool IsActive => Parent.IsActive;

        public int FrameCount=> Parent.FrameCount;



        public DateTime EndFrame => Parent.EndFrame;
        public DateTime StartFrame => Parent.StartFrame;

        public DateTime GameStarted { get; protected set; }
        public TimeSpan Elapsed => DateTime.Now - GameStarted;




        public float FramesPerSecond => Parent.FramesPerSecond;

        public abstract void Init();

        public virtual void Reset()
        {
            GameStarted = DateTime.Now;

        }
        
        public abstract void Step(float elapsedSec);

        public abstract void Draw();
        
        public abstract void Dispose();
    }
}