using Love;
using Match3.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Scene
{
    public class TileSlot
    {
        public const float TILE_WORLD_SIZE = 20f;
        public Board Board { get; private set; }
        public BasicTile Tile { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public RectangleF Bounds => new RectangleF(X * TILE_WORLD_SIZE, (Board.RowCount - 1 - Y) * TILE_WORLD_SIZE, TILE_WORLD_SIZE, TILE_WORLD_SIZE);
        public Vector2 Position => Bounds.Center;
        public bool Empty => Tile == null;
        public TileSlot Up => GetNeighbour(0, 1);
        public TileSlot Down => GetNeighbour(0, -1);
        public TileSlot Right => GetNeighbour(1, 0);
        public TileSlot Left => GetNeighbour(-1, 0);

        public TileSlot(Board board, int x, int y)
        {
            Board = board;
            X = x;
            Y = y;
        }
        public TileSlot GetNeighbour(int dX, int dY)
        {
            if (Board != null)
            {
                var x = X + dX;
                var y = Y + dY;
                if (x >= 0 && x < Board.ColumnCount && y >= 0 && y < Board.RowCount)
                {
                    return Board.Slots[x, y];
                }
            }
            return null;
        }
        public List<TileSlot> GetNeighboursInRadius(int radius)
        {
            var result = new List<TileSlot>();
            if (Board != null)
            {
                var xStart = Math.Max(X - radius, 0);
                var yStart = Math.Max(Y - radius, 0);
                var xEnd = Math.Min(X + radius + 1, Board.ColumnCount);
                var yEnd = Math.Min(Y + radius + 1, Board.RowCount);

                for (int x = xStart; x < xEnd; x++)
                {
                    for (int y = yStart; y < yEnd; y++)
                    {
                        result.Add(Board.Slots[x, y]);
                    }
                }
            }
            return result;
        }
    }
}
