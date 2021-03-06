using Match3.Basics;
using System;
using System.Collections.Generic;
using System.Text;
using Match3.Tiles;
using Match3.Scene;
using Love;

namespace Match3.Animation
{
    public class MoveToSlotAnimation : BasicAnimation
    {
        public float MovingSpeed { get; set; }
        public TileSlot Destination { get; set; }
        public MoveToSlotAnimation(Entity entity, TileSlot slot, float moveSpeed) : base(entity)
        {
            MovingSpeed = moveSpeed;
            Destination = slot;
        }
        public override void Update(float deltaTime)
        {
            var delta = Destination.Position - Owner.Position;
            var remainTime = (delta.Length() / TileSlot.TILE_WORLD_SIZE) / MovingSpeed;
            Owner.Position = Vector2.Lerp(Owner.Position, Destination.Position, deltaTime / remainTime);

            if (Destination.Position == Owner.Position)
            {
                Finish();
            }
        }
    }
}
