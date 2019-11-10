using System;
using System.IO;
using System.Threading;
using Tetris.Lib.Logic;
using Tetris.Lib.Math;

namespace Tetris.Lib.Rendering
{
    public class ResourceManager
    {
        public string RootDir { get; set; }

        public ResourceManager()
        {
            var source = @"C:\Projects\RetroTetris\src\Tetris.Console\res";
            if (Directory.Exists( source))
            {
                RootDir = source;
                return;
            }

            if (Directory.GetCurrentDirectory().ToLowerInvariant().Trim('\\').EndsWith("res"))
            {
                RootDir = "./";
                return;
            }

            var sub = Path.Combine(Directory.GetCurrentDirectory(), "res");
            if (Directory.Exists(sub))
            {
                RootDir = sub;
                return;
            }


            throw new Exception("Cannot find asserts");
        }


        public string GetResource(string relfile) => Path.Combine(RootDir, relfile);
    }
    
    

   

}