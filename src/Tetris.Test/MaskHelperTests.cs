using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Lib.Math;
using Xunit;
using Xunit.Abstractions;

namespace Tetris.Lib.Tests
{
    public class MaskHelperTests
    {
        private readonly ITestOutputHelper outp;

        public MaskHelperTests(ITestOutputHelper outp)
        {
            this.outp = outp;
        }

        [Fact]
        public void RotateRight()
        {
            var a = Mask.Create(new []{"###", ".#."});
            outp.WriteLine(a.RotateRight(0).ToMultiLineString());
            outp.WriteLine(a.RotateRight(1).ToMultiLineString());
            outp.WriteLine(a.RotateRight(2).ToMultiLineString());
            outp.WriteLine(a.RotateRight(3).ToMultiLineString());

            Assert.Equal("#.|##|#.|", a.RotateRight(3).ToSingleString());
            Assert.Equal(".#.|###|",  a.RotateRight(2).ToSingleString());
            Assert.Equal(".#|##|.#|", a.RotateRight(1).ToSingleString());
        }

        [Fact]
        public void RotateLeft()
        {
            var a = Mask.Create(new []{"###", ".#."});

            outp.WriteLine(a.RotateLeft(0).ToMultiLineString());
            outp.WriteLine(a.RotateLeft(1).ToMultiLineString());
            outp.WriteLine(a.RotateLeft(2).ToMultiLineString());
            outp.WriteLine(a.RotateLeft(3).ToMultiLineString());

            Assert.Equal("#.|##|#.|", a.RotateLeft(1).ToSingleString());
            Assert.Equal(".#.|###|",  a.RotateLeft(2).ToSingleString());
            Assert.Equal(".#|##|.#|", a.RotateLeft(3).ToSingleString());
        }

        
        [Fact]
        public void RotateRight_L()
        {
            var a = Mask.Create(new []{"###", "..#"});

            outp.WriteLine(a.RotateRight(0).ToMultiLineString());
            outp.WriteLine(a.RotateRight(1).ToMultiLineString());
            outp.WriteLine(a.RotateRight(2).ToMultiLineString());
            outp.WriteLine(a.RotateRight(3).ToMultiLineString());

            Assert.Equal(".#|.#|##|", a.RotateRight(1).ToSingleString());
            Assert.Equal("#..|###|",  a.RotateRight(2).ToSingleString());
            Assert.Equal("##|#.|#.|", a.RotateRight(3).ToSingleString());

            
        }


        [Fact]
        public void RotateRight_Line()
        {
            var a = Mask.Create(new []{"####"});
            Assert.Equal("#|#|#|#|", a.RotateRight(1).ToSingleString());
            
        }

       
        
    }
}
