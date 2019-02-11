using System;


namespace Tetris.Lib.Util
{
    public static class StringHelper
    {
        public static (string l, string r) SplitEitherSize(string txt, string split)
        {
            var i = txt.IndexOf(split);
            return (txt.Substring(0, i), txt.Remove(i + split.Length));
        }
    }
}