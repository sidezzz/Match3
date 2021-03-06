using Match3.Basics;
using Match3.Scene;
using System;
using System.Collections.Generic;
using System.Text;
using Match3.Animation;
using Love;

namespace Match3.BonusEntities
{
    public class LineBonus : BasicBonusEntity
    {
        Board _board;
        public LineBonus(TileSlot spawnPoint, int xDir, int yDir, float moveSpeed) : base()
        {
            _board = spawnPoint.Board;
            Position = spawnPoint.Position;

            var destination = spawnPoint;
            while (true)
            {
                var next = destination.GetNeighbour(xDir, yDir);
                if (next != null)
                {
                    destination = next;
                }
                else
                {
                    break;
                }
            }

            Animation = new MoveToSlotAnimation(this, destination, moveSpeed);
        }
        public override void Draw()
        {
            Graphics.SetColor(Color.White);
            Graphics.Circle(DrawMode.Fill, 0, 0, TileSlot.TILE_WORLD_SIZE / 4f, 30);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            foreach (var slot in _board.Slots)
            {
                if (slot.Tile?.Bounds.Contains(Position) == true)
                {
                    slot.Tile.Match();
                }
            }

            if (Animation.State == AnimationState.Finished)
            {
                Destroy();
            }
        }
    }
}
