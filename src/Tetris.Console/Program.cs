using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Tetris.Lib.Logic;
using Tetris.Lib.Math;
using Tetris.Lib.Rendering;

namespace Tetris.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestAudio();
            //KeyCapture();
          
            /* NOTE: Tetris.Lib has no sound support.
             * So sound functionality is implemented here
             *
             */
            var render = new TextConsoleRenderer(new IntVector2(80, 40));
            using (var loop = new MasterGameLoop(render))
            {
                // on startup:
                var music = new CachedSound( loop.ResourceManager.GetResource("music/Tetris_theme.mp3"));
                var addFloor = new CachedSound( loop.ResourceManager.GetResource("sfx/AddFloor.mp3"));
                var lineCompelete = new CachedSound( loop.ResourceManager.GetResource("sfx/LineComplete.mp3"));

                // later in the app...
                AudioPlaybackEngine.Instance.PlaySound(music);
                loop.Init();

                if (args.Any())
                {
                    if (args[0].ToLowerInvariant() == "-j")
                    {
                        loop.Tetris.KeyA = TetrisGameLoop.KeyXArcadeLeft;
                        loop.Tetris.KeyB = TetrisGameLoop.KeyXArcadeRight;
                    }
                }

                loop.Tetris.GameA.OnEvent += (sender, ge) =>
                {
                    if (ge.Event == TetrisEvents.AddFloor)
                    {
                        AudioPlaybackEngine.Instance.PlaySound(addFloor);
                    }
                    if (ge.Event == TetrisEvents.LineComplete)
                    {
                        AudioPlaybackEngine.Instance.PlaySound(lineCompelete);
                    }
                    
                };

                loop.Start();
            }
        }
    }
}
