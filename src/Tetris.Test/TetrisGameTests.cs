using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tetris.Lib.Logic;
using Xunit;
using Xunit.Abstractions;

namespace Tetris.Test
{
    public class TetrisGameTests
    {
        private readonly ITestOutputHelper outp;

        public TetrisGameTests(ITestOutputHelper outp)
        {
            this.outp = outp;
        }

        [Fact]
        public void CanLoadState()
        {
            

            var json = @"{
  ""State"": 1,
  ""Floor"": [
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    "".L.........J"",
    "".L....SS...J"",
    "".LL..SS...JJ""
  ],
  ""ActiveT"": ""O"",
  ""ActiveX"": 5,
  ""ActiveY"": 5,
  ""ActiveR"": 0,
  ""Speed"": 0,
  ""Score"": 20,
  ""NextT"": ""T""
}";

            outp.WriteLine(json);

            var game = new TetrisGame();
            game.Load(JsonConvert.DeserializeObject<TetrisGameStateDto>(json));


            var round = JsonConvert.SerializeObject(game.CaptureState(), Formatting.Indented);
            outp.WriteLine(round);

            Assert.Equal(json, round);

        }

        [Fact]
        public void LineComplete()
        {
            var json = @"{
  ""State"": 1,
  ""Floor"": [
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""............"",
    ""....T......."",
    ""....TT.OO..I"",
    ""....TT.OOI.I"",
    ""...TTT.OOI.I"",
    ""I..TTT.OOI.I"",
    ""I..TOO.JJIOO"",
    ""I..SOOTJJJOO"",
    ""I.SSSTTJJSLL"",
    ""J.SSSTTJJSSL""
  ],
  ""ActiveT"": ""J"",
  ""ActiveX"": 1,
  ""ActiveY"": 15,
  ""ActiveR"": 3,
  ""Speed"": 0,
  ""Score"": 205,
  ""NextT"": ""S""
}";
            var game = new TetrisGame();
            game.Load(JsonConvert.DeserializeObject<TetrisGameStateDto>(json));

            game.SendInput(Input.Drop);

            // Check no left right
            Assert.False(game.SendInput(Input.MoveRight));
            Assert.False(game.SendInput(Input.MoveLeft));

            game.Step();

        }
    }
}
