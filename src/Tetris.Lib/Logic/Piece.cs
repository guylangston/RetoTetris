using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Lib.Math;

namespace Tetris.Lib.Logic
{
    public class Piece
    {
        public Piece(TetrisGame game, Tetromino tetromino)
        {
            Game = game;
            Tetromino = tetromino;
        }

        public TetrisGame Game { get; }
        public Tetromino Tetromino { get; }
        public int Rotation { get; set; }
        public IntVector2 Position { get; set; }
        
        public IMask GetLocalBricks() => Tetromino.Bricks.RotateRight(Rotation);
        public IEnumerable<IntVector2> GetBricks() => GetLocalBricks().Select(x=>x + Position);
        public IEnumerable<IntVector2> GetBricks(int r) => Tetromino.Bricks.RotateRight(r).Select(x=>x + Position);
        public IEnumerable<IntVector2> GetBricks(IntVector2 p, int r) => Tetromino.Bricks.RotateRight(r).Select(x=>x + p);
    }
}