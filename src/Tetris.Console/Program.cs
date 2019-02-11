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
          
            var render = new TextConsoleRenderer(new IntVector2(80, 40));
            using (var loop = new MasterGameLoop(render))
            {
                if (args.Any())
                {
                    if (args[0].ToLowerInvariant() == "-j")
                    {
                        loop.Tetris.KeyA = TetrisGameLoop.KeyXArcadeLeft;
                        loop.Tetris.KeyB = TetrisGameLoop.KeyXArcadeRight;
                    }
                }

                // on startup:
                var music = new CachedSound( loop.ResourceManager.GetResource("music/Tetris_theme.mp3"));
                var addFloor = new CachedSound( loop.ResourceManager.GetResource("sfx/AddFloor.mp3"));
                var lineCompelete = new CachedSound( loop.ResourceManager.GetResource("sfx/LineComplete.mp3"));

                // later in the app...
                AudioPlaybackEngine.Instance.PlaySound(music);
                loop.Init();

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
         

                

          
          ////var sfxA = new AudioFileReader(@"C:\Projects\EulerProject\NonEuler\Tetris\res\sounds\162467__kastenfrosch__gotitem.mp3");
          //  // https://markheath.net/post/fire-and-forget-audio-playback-with
          //  using(var audioFile = new AudioFileReader(@".\res\music\Tetris_theme.mp3"))
          //  using(var outputDevice = new WaveOutEvent())
          //  {
          //      LoopStream loop = new LoopStream(audioFile);

          //      outputDevice.Volume = 0.3f;
          //      outputDevice.Init(loop);
          //      outputDevice.Play();

               
               
          //  }


        }

       

        static void GameAOnOnEvent(object sender, GameEventArgs ge)
        {
                    
                   
        }

        /// <summary>
        /// Stream for looping playback
        /// </summary>
        public class LoopStream : WaveStream
        {
            WaveStream sourceStream;

            /// <summary>
            /// Creates a new Loop stream
            /// </summary>
            /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
            /// or else we will not loop to the start again.</param>
            public LoopStream(WaveStream sourceStream)
            {
                this.sourceStream = sourceStream;
                this.EnableLooping = true;
            }

            /// <summary>
            /// Use this to turn looping on or off
            /// </summary>
            public bool EnableLooping { get; set; }

            /// <summary>
            /// Return source stream's wave format
            /// </summary>
            public override WaveFormat WaveFormat
            {
                get { return sourceStream.WaveFormat; }
            }

            /// <summary>
            /// LoopStream simply returns
            /// </summary>
            public override long Length
            {
                get { return sourceStream.Length; }
            }

            /// <summary>
            /// LoopStream simply passes on positioning to source stream
            /// </summary>
            public override long Position
            {
                get { return sourceStream.Position; }
                set { sourceStream.Position = value; }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int totalBytesRead = 0;

                while (totalBytesRead < count)
                {
                    int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        if (sourceStream.Position == 0 || !EnableLooping)
                        {
                            // something wrong with the source stream
                            break;
                        }
                        // loop
                        sourceStream.Position = 0;
                    }
                    totalBytesRead += bytesRead;
                }
                return totalBytesRead;
            }
        }

        private static void TestAudio()
        {

            // on startup:
            var zap = new CachedSound(@"C:\Projects\EulerProject\NonEuler\Tetris\res\music\Tetris_theme.mp3");
            var boom = new CachedSound(@"C:\Projects\EulerProject\NonEuler\Tetris\res\sounds\162464__kastenfrosch__message.mp3");

            // later in the app...
            AudioPlaybackEngine.Instance.PlaySound(zap);
            
            
            ConsoleKeyInfo k = new ConsoleKeyInfo();
            while (k.Key != ConsoleKey.Escape)
            {
                if (Console.KeyAvailable)
                {
                    k = Console.ReadKey();

                    if (k.KeyChar == '1')
                    {
                        AudioPlaybackEngine.Instance.PlaySound(boom);
                    }
                }

                
            }
            

            Console.ReadLine();

            // on shutdown
            AudioPlaybackEngine.Instance.Dispose();

            //using(var audioFile = new AudioFileReader(@"C:\Projects\EulerProject\NonEuler\Tetris\res\music\Tetris_theme.mp3"))
            //using(var outputDevice = new WaveOutEvent())
            //{
            //    outputDevice.Init(audioFile);
            //    outputDevice.Play();
            //    while (outputDevice.PlaybackState == PlaybackState.Playing)
            //    {
            //        Thread.Sleep(1000);
            //    }
            //}
        }

        private static void TestAudio2()
        {
            var sine20Seconds = new SignalGenerator() { 
                    Gain = 0.2, 
                    Frequency = 500,
                    Type = SignalGeneratorType.Sin}
                .Take(TimeSpan.FromSeconds(20));
            using (var wo = new WaveOutEvent())
            {
                wo.Init(sine20Seconds);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(500);
                }
            }
        }

        private static void KeyCapture()
        {
            

            while (true)
            {
                var x = Console.ReadKey();
                Console.WriteLine($"{x.Key}, {x.KeyChar}, {x.Modifiers}");

                if (x.Key == ConsoleKey.Escape) return;
            }
        }
    }
}
