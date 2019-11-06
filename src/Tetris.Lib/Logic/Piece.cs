using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Lib.Math;
using VectorInt;

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
        public VectorInt2 Position { get; set; }
        
        public IMask GetLocalBricks() => Tetromino.Bricks.RotateRight(Rotation);
        public IEnumerable<VectorInt2> GetBricks() => GetLocalBricks().Select(x=>x + Position);
        public IEnumerable<VectorInt2> GetBricks(int r) => Tetromino.Bricks.RotateRight(r).Select(x=>x + Position);
        public IEnumerable<VectorInt2> GetBricks(VectorInt2 p, int r) => Tetromino.Bricks.RotateRight(r).Select(x=>x + p);
    }
}