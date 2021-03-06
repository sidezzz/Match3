using System;
using System.Collections.Generic;
using System.Text;
using Match3.Scene;
using Love;
using Match3.Tiles;

namespace Match3.Animation
{
    public class FallAnimation : BasicAnimation
    {
        float _fallSpeed;
        MoveToSlotAnimation _moveAnim;
        public FallAnimation(BasicTile tile, float fallSpeed) : base(tile)
        {
            _fallSpeed = fallSpeed;
        }
        public override void Update(float deltaTime)
        {
            var tile = Owner as BasicTile;
            var targetSlot = tile.Slot;
            while (targetSlot.Down != null && targetSlot.Down.Empty)
            {
                targetSlot = targetSlot.Down;
            }

            if (targetSlot != tile.Slot)
            {
                tile.AssignToSlot(targetSlot, false);
                _moveAnim = new MoveToSlotAnimation(Owner, targetSlot, _fallSpeed);
                BlocksInput = true;
            }

            _moveAnim?.Update(deltaTime);
            if (_moveAnim == null || _moveAnim.State == AnimationState.Finished)
            {
                BlocksInput = false;
            }
        }
    }
}
