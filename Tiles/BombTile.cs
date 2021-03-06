using System;
using System.Collections.Generic;
using System.Text;
using Love;
using Match3.BonusEntities;
using Match3.Scene;

namespace Match3.Tiles
{
    public class BombTile : ColoredTile
    {
        bool _exploded = false;
        public BombTile(ColoredTile templateTile) : base(templateTile.Color, "")
        {
            MatchTypeName = templateTile.MatchTypeName;
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.Circle(DrawMode.Fill, 0, 0, TileSlot.TILE_WORLD_SIZE / 4f, 30);
        }

        public override void Match()
        {
            base.Match();

            if (!_exploded)
            {
                Slot.Board.Entities.Add(new BombBonus(Slot, 1, 0.25f));
                _exploded = true;
            }
        }
    }
}
