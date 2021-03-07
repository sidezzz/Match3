using System;
using System.Collections.Generic;
using System.Text;
using Love;
using Match3.Scene;

namespace Match3
{
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }
    public class Game : Love.Scene
    {
        public Board Board { get; private set; }
        public GameState State { get; private set; } = GameState.MainMenu;
        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }
        Rectangle PlayButtonRect => new Rectangle(WindowWidth / 4, WindowHeight / 10, WindowWidth / 2, WindowHeight / 5);
        Rectangle OkButtonRect => new Rectangle(WindowWidth / 4, WindowHeight / 4, WindowWidth / 2, WindowHeight / 7);
        public float RemainTime { get; private set; }
        Font _uiFont;
        Font _scoreFont;
        public Game(Board board, int width, int height)
        {
            Board = board;
            WindowWidth = width;
            WindowHeight = height;
        }
        public override void Load()
        {
            base.Load();

            _uiFont = Graphics.NewFont("C:\\Windows\\Fonts\\Tahoma.ttf", WindowHeight / 9);
            _scoreFont = Graphics.NewFont("C:\\Windows\\Fonts\\Tahoma.ttf", Board.TILE_PIXEL_SIZE / 2);
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.SetBackgroundColor(Color.CadetBlue);

            switch (State)
            {
                case GameState.MainMenu:
                    {
                        Graphics.SetColor(Color.White);
                        Graphics.Rectangle(DrawMode.Fill, PlayButtonRect);
                        Graphics.SetColor(Color.Black);
                        var text = Graphics.NewText(_uiFont, "Play");
                        Graphics.Draw(text, PlayButtonRect.Center.X - text.GetWidth() / 2, PlayButtonRect.Center.Y - text.GetHeight() / 2);
                        break;
                    }
                case GameState.GameOver:
                    {
                        Board.Draw();
                        Graphics.SetColor(Color.White);
                        Graphics.Rectangle(DrawMode.Fill, OkButtonRect);
                        Graphics.SetColor(Color.Black);
                        var gameOverText = Graphics.NewText(_uiFont, "Game Over");
                        Graphics.Draw(gameOverText, OkButtonRect.Center.X - gameOverText.GetWidth() / 2, OkButtonRect.Top - gameOverText.GetHeight());
                        var okText = Graphics.NewText(_uiFont, "Ok");
                        Graphics.Draw(okText, OkButtonRect.Center.X - okText.GetWidth() / 2, OkButtonRect.Center.Y - okText.GetHeight() / 2);
                        var scoreText = Graphics.NewText(_scoreFont, $"Score:{Board.Score} Time:{RemainTime}");
                        Graphics.Draw(scoreText, 0, Board.RowCount * Board.TILE_PIXEL_SIZE);
                        break;
                    }
                case GameState.Playing:
                    {
                        Board.Draw();
                        Graphics.SetColor(Color.Black);
                        var scoreText = Graphics.NewText(_scoreFont, $"Score:{Board.Score} Time:{RemainTime.ToString("0.##")}");
                        Graphics.Draw(scoreText, 0, Board.RowCount * Board.TILE_PIXEL_SIZE);
                        break;
                    }
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (State == GameState.Playing)
            {
                RemainTime = Math.Max(RemainTime - dt, 0f);
                if (RemainTime == 0f)
                {
                    State = GameState.GameOver;
                }
                else
                {
                    Board.Update(dt);
                }
            }
        }

        public override void MousePressed(float x, float y, int button, bool isTouch)
        {
            base.MousePressed(x, y, button, isTouch);

            switch (State)
            {
                case GameState.MainMenu:
                    if (PlayButtonRect.Contains((int)x, (int)y))
                    {
                        RemainTime = 60f;
                        State = GameState.Playing;
                    }
                    break;
                case GameState.GameOver:
                    Board.Draw();
                    if (OkButtonRect.Contains((int)x, (int)y))
                    {
                        State = GameState.MainMenu;
                        Board.Restart();
                    }
                    break;
                case GameState.Playing:
                    Board.MousePressed(x, y, button);
                    break;
            }
        }
    }
}
