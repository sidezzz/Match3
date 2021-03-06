using System;
using System.Collections.Generic;
using System.Text;
using Love;
using Match3.Scene;

namespace Match3.Tiles
{
    public class CircleTile : ColoredTile
    {
        public CircleTile(Color color) : base(color, "Circle")
        {
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.Circle(DrawMode.Fill, 0, 0, TileSlot.TILE_WORLD_SIZE / 2f - 1, 30);
        }
    }
}
