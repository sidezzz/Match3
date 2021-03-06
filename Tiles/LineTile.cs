using Love;
using Match3.BonusEntities;
using Match3.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Tiles
{
    public class LineTile : ColoredTile
    {
        bool _exploded = false;
        int _xDir;
        int _yDir;
        public LineTile(ColoredTile templateTile, int xDir, int yDir) : base(templateTile.Color, "")
        {
            MatchTypeName = templateTile.MatchTypeName;
            _xDir = xDir;
            _yDir = yDir;
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.Circle(DrawMode.Line, 0, 0, TileSlot.TILE_WORLD_SIZE / 4f, 30);


            var lineOffset = new Vector2(TileSlot.TILE_WORLD_SIZE / 3f * _xDir, TileSlot.TILE_WORLD_SIZE / 3f * -_yDir);
            Graphics.Line(lineOffset, new Vector2(-lineOffset.X, -lineOffset.Y));
        }

        public override void Match()
        {
            base.Match();

            if (!_exploded)
            {
                Slot.Board.Entities.Add(new LineBonus(Slot, _xDir, _yDir, 7f));
                Slot.Board.Entities.Add(new LineBonus(Slot, -_xDir, -_yDir, 7f));
                _exploded = true;
            }
        }
    }
}
