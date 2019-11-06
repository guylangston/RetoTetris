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
    
    

   

}