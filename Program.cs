using System;
using Love;
using Match3.Matchers;
using Match3.Misc;
using Match3.Scene;

namespace Match3
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(18, 18);
            board.Matchers.Add(new LineMatcher(1, 0));
            board.Matchers.Add(new LineMatcher(0, -1));

            //board.Matchers.Add(new LineMatcher(1, -1));
            //board.Matchers.Add(new LineMatcher(-1, -1));
            // uncomment to get almost self playing game

            board.TileGenerator = new RandomTileGenerator();

            var config = new BootConfig
            {
                WindowWidth = board.ColumnCount * Board.TILE_PIXEL_SIZE,
                WindowHeight = (board.RowCount + 1) * Board.TILE_PIXEL_SIZE,
                WindowTitle = "Match3"
            };

            Boot.Init(config);

            Boot.Run(new Game(board, config.WindowWidth, config.WindowHeight));
        }
    }
}
