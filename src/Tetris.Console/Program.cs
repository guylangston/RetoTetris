using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleZ.Drawing;
using ConsoleZ.Drawing.Game;
using ConsoleZ.Win32;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Tetris.Lib.Logic;
using Tetris.Lib.Math;
using Tetris.Lib.Rendering;
using VectorInt;

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
            var render = new TextConsoleRenderer(new VectorInt2(80, 40));
            render.Init();


            using (var masterLoop = new ConsoleGameLoop<ConsolePixel>(new InputProvider(), render))
            {
                using (var loop = new MasterGameLoop(masterLoop))
                {
                    // on startup:
                    var music         = new CachedSound( loop.ResourceManager.GetResource("music/Tetris_theme.mp3"));
                    var addFloor      = new CachedSound( loop.ResourceManager.GetResource("sfx/AddFloor.mp3"));
                    var lineCompelete = new CachedSound( loop.ResourceManager.GetResource("sfx/LineComplete.mp3"));
                    
                    if (args.Any())
                    {
                        if (args[0].ToLowerInvariant() == "-j")
                        {
                            loop.Tetris.KeyA = TetrisGameLoop.KeyXArcadeLeft;
                            loop.Tetris.KeyB = TetrisGameLoop.KeyXArcadeRight;
                        }
                    }

                    masterLoop.Scene = loop;
                    masterLoop.Init();    // will init loop
                    
                    // After init...
                    AudioPlaybackEngine.Instance.PlaySound(music);
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
                    
                    masterLoop.Start();
                }
                
            }
            
            
        }
    }
}
