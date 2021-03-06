using Love;
using Match3.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Tiles
{
    public class RectangleTile : ColoredTile
    {
        public RectangleTile(Color color) : base(color, "Rectangle")
        {
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.Rectangle(DrawMode.Fill, -TileSlot.TILE_WORLD_SIZE / 2 + 1, -TileSlot.TILE_WORLD_SIZE / 2 + 1, TileSlot.TILE_WORLD_SIZE - 2, TileSlot.TILE_WORLD_SIZE - 2);
        }
    }
}
