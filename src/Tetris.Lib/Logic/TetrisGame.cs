using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Lib.Math;
using Tetris.Lib.Util;
using VectorInt;


namespace Tetris.Lib.Logic
{
    public enum GameState
    {
        WaitingStart,
        InProgress,
        Paused,
        GameOver
    }

    public enum PlayState
    {
        None,
        LineComplete
    }

    public enum Input
    {
        Start,
        Pause,
        
        MoveLeft,
        MoveRight,
        Rotate,
        Drop,

        Save,
        
        Quit
    }

    public class GameEventArgs : EventArgs
    {
        public TetrisGame Game { get; set; }
        public TetrisEvents Event { get; set; }
        public string Tag { get; set; }
    }

    public delegate void GameEvent(object sender, GameEventArgs ge);

    /// <summary>
    /// Json serialisable game-state
    /// </summary>
    public class TetrisGameStateDto
    {
        public GameState State { get; set; }

        public string[] Floor { get; set; }

        public char ActiveT { get; set; } // Tetromino
        public int ActiveX { get; set; }
        public int ActiveY { get; set; }
        public int ActiveR { get; set; } // Rotation

        public int Speed { get; set; }
        public int Score { get; set; }

        public char NextT { get; set; } // Tetromino
        
    }

    public enum TetrisEvents
    {
        NewPiece,
        AddFloor,
        Tetris,
        LineComplete,
        GameOver
    }


    // https://en.wikipedia.org/wiki/Tetris
    public class TetrisGame
    {
        private Matrix2<Tetromino> floor;
        
        public int Score { get; private set; }
        public int Speed { get; private set; }
        public int Width { get; } = 12;
        public int Height { get; } = 18;
        public GameState State { get; private set; }
        public PlayState PlayState { get; private set; }
      
        public Piece Active { get; private set; }
        public Tetromino Next { get; private set; }
        public int StepCount { get; private set; }
        public string Message { get; set; }

        public int ScorePerPiece { get; set; } = 5;
        public int ScorePerLine { get; set; } = 100;
        public IMatrix2<Tetromino> Floor => floor;

        // Stats
        public int PieceCount { get; private set; }
        public int Lines { get; private set; }

        public event GameEvent OnEvent;


        public TetrisGame()
        {
            State = GameState.WaitingStart;
            floor = new Matrix2<Tetromino>((Width, Height));
        }

        public void Load(TetrisGameStateDto dto)
        {
            floor = Matrix2<Tetromino>.Create(dto.Floor, Tetromino.GetByChar);
            State = dto.State;

            Active = new Piece(this, Tetromino.All.First(x => x.Name == dto.ActiveT))
            {
                Position = new VectorInt2(dto.ActiveX, dto.ActiveY), 
                Rotation = dto.ActiveR
            };

            Speed = dto.Speed;
            Score = dto.Score;

            Next = Tetromino.GetByChar(dto.NextT);
        }

        public TetrisGameStateDto CaptureState()
        {
            return new TetrisGameStateDto()
            {
                State = State,

                ActiveT = Active.Tetromino.Name,
                ActiveX = Active.Position.X,
                ActiveY = Active.Position.Y,
                ActiveR = Active.Rotation,

                NextT = Next.Name,
                Speed = Speed,
                Score = Score,

                Floor = CaptureFloor()
            };
        }

        public string[] CaptureFloor() => floor?.ToString(x => x?.Name ?? '.').ToArray();

        public int LineCompleted { get; set; } = -1;

       

        public void Step()
        {
            StepCount++;
            if (State == GameState.GameOver) return;

            if (State == GameState.InProgress)
            {
                if (PlayState == PlayState.None)
                {
                    if (TouchesFloorPile(Active))
                    {
                        AddToFloor(Active);
                        Active = null;

                        LineCompleted = FirstCompleteLine();
                        if (LineCompleted != -1)
                        {
                            PlayState = PlayState.LineComplete;
                            return;
                        }

                        AddNewPiece();
                    }
                    else
                    {
                        Active.Position = Active.Position.AddY(1);
                    }
                }
                else if (PlayState == PlayState.LineComplete)
                {
                    LineCompleted = FirstCompleteLine();
                    if (LineCompleted != -1)
                    {
                        Score += ScorePerLine;
                        EraseLine(LineCompleted);
                        Lines++;
                        SendEvent(TetrisEvents.LineComplete);

                        LineCompleted = FirstCompleteLine();

                    }
                    else
                    {
                        PlayState = PlayState.None;
                        AddNewPiece();
                    }
                }
            }
        }

        private void AddNewPiece()
        {
            Active = CreatePiece(Next);
            Next = RandomHelper.ChooseOne(Tetromino.All);

            if (TouchesFloorPile(Active))
            {
                // Game Over
                State = GameState.GameOver;
                SendEvent(TetrisEvents.GameOver);
            }
            else
            {
                SendEvent(TetrisEvents.NewPiece);
            }
        }

        private void SendEvent(TetrisEvents type)
        {
            if (OnEvent != null)
            {
                OnEvent(this, new GameEventArgs()
                {
                    Game = this,
                    Event = type
                });
            }
        }

        private void AddToFloor(Piece piece)
        {
            SendEvent(TetrisEvents.AddFloor);
            foreach (var p in piece.GetBricks())
            {
                floor[p] = piece.Tetromino;
            }
        }

        private void EraseLine(int line)
        {
            var y = line;
            while (y > 0)
            {
                for (var x = 0; x < floor.Size.X; x++)
                {
                    floor[x, y] = floor[x, y-1];
                }
                y--;
            }
        }
        
        private int FirstCompleteLine()
        {
            for (int y = Height; y > 0; y--)
            {
                var complete = Enumerable.Range(0, Width).All(x=>floor[x,y] != null);
                if (complete)
                {
                    return y;
                }
            }
            return -1;
        }

        private Piece CreatePiece(Tetromino next)
        {
            Score += ScorePerPiece;
            PieceCount++;
            return new Piece(this, next)
            {
                Position = new VectorInt2(Width/2, 0),
                Rotation = 0
            };
        }

        private bool TouchesFloorPile(Piece active)
        {
            if (active.GetBricks().Any(x=>x.Y == Height-1)) return true;
            var act = Active.GetBricks().ToArray();

            // Bottom touch
            if (act.Any(a => floor[a.AddY(1)] != null))
            {
                return true;
            }
            
            return false;
        }

        static bool Collision(ICollection<VectorInt2> a, ICollection<VectorInt2> b)
        {
            foreach (var aa in a)
            {
                foreach (var bb in b)
                {
                    if (aa.Equals(bb)) return true;
                }
            }

            return false;
        }

        public bool SendInput(Input keyPress)
        {
            if (State == GameState.InProgress)
            {
                if (Active != null)
                {
                    if (keyPress == Input.MoveRight)
                    {
                        if (Active.GetBricks().All(a => a.X < Width-1)  // wall
                            && !Collision(floor.ForEach().ToArray(), Active.GetBricks(Active.Position.AddX(1), Active.Rotation).ToArray())
                        )
                        {
                            Active.Position = Active.Position.AddX(1);
                            return true;
                        }
                        return false;
                    }   
                    else if (keyPress == Input.MoveLeft)
                    {
                        if (Active.GetBricks().All(a => a.X > 0)
                            && !Collision(floor.ForEach().ToArray(), Active.GetBricks(Active.Position.AddX(-1), Active.Rotation).ToArray())
                        )
                        {
                            Active.Position = Active.Position.AddX(-1);
                            return true;
                        }
                        return false;
                    }
                    else if (keyPress == Input.Rotate)
                    {
                        if (Active.GetBricks(Active.Rotation + 1).All(a => a.X >= 0 &&  a.X < Width))
                        {
                            Active.Rotation++;
                            return true;
                        }
                        return false;
                    }
                    else if (keyPress == Input.Drop)
                    {
                        while (!TouchesFloorPile(Active))
                        {
                            Active.Position = Active.Position.AddY(1);
                        }
                        return true;
                    }
                }
                
                if (keyPress == Input.Quit)
                {
                    SetMessage("Quitting... Bye Bye!");
                    State = GameState.GameOver;
                    return true;
                }
                
            }
            else if (State == GameState.WaitingStart && keyPress == Input.Start)
            {
                SetMessage("Good Luck...");
                State = GameState.InProgress;
                Next = RandomHelper.ChooseOne(Tetromino.All);
                Active = CreatePiece(Next);
                Next = RandomHelper.ChooseOne(Tetromino.All);
                return true;
            }
            return true;
        }

        private void SetMessage(string message)
        {
            Message = message;
        }




        
    }
}