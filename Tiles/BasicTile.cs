using System;
using System.Collections.Generic;
using System.Text;
using Love;
using Match3.Scene;
using Match3.Basics;
using Match3.Animation;

namespace Match3.Tiles
{
    public abstract class BasicTile : Entity
    {
        public const float DEFAULT_FALL_SPEED = 4f; // tiles per second
        public TileSlot Slot { get; private set; }
        public RectangleF Bounds => new RectangleF(Position.X - TileSlot.TILE_WORLD_SIZE / 2, Position.Y - TileSlot.TILE_WORLD_SIZE / 2, TileSlot.TILE_WORLD_SIZE, TileSlot.TILE_WORLD_SIZE);
        public Board Board => Slot?.Board;
        public bool EnableFalling { get; set; } = true;
        public string MatchTypeName { get; protected set; }

        public BasicTile()
        {
            Animation = new FadeInAnimation(this, 0.5f);
        }
        public virtual void Match()
        {
            if (!(Animation is MatchAnimation))
            {
                Animation = new MatchAnimation(this, 0.2f);
            }
        }
        public override void Destroy()
        {
            base.Destroy();
            if (Slot != null && Slot.Tile == this)
            {
                Slot.Tile = null;
            }
            Slot = null;
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (EnableFalling)
            {
                if (Animation == null || (!(Animation is FallAnimation) && Animation.State == AnimationState.Finished))
                {
                    Animation = new FallAnimation(this, DEFAULT_FALL_SPEED);
                    Animation.Update(deltaTime);
                }
                //if (Slot?.Down?.Empty == true)
                //{
                //    if (Animation is FallAnimation)
                //    {
                //        Animation.Resume();
                //    }
                //    else if (Animation.State == AnimationState.Finished)
                //    {
                //        Animation = new FallAnimation(this, DEFAULT_FALL_SPEED);
                //    }
                //}
            }
        }
        public void StartSelectAnimation()
        {
            DrawPriority = DrawPriority.Selected;
            Animation = new SelectionAnimation(this);
        }
        public void StopSelectAnimation()
        {
            DrawPriority = DrawPriority.Default;
            if (Animation is SelectionAnimation)
            {
                Animation.Finish();
            }
        }
        public void AssignToSlot(TileSlot slot, bool adjustPosition = true)
        {
            if (Slot != null)
            {
                Slot.Tile = null;
            }

            slot.Tile = this;
            Slot = slot;
            if (adjustPosition)
            {
                Position = Slot.Position;
            }
        }
        public void SwapWith(BasicTile tile, bool adjustPosition = true)
        {
            var slot = tile.Slot;
            tile.Slot = Slot;
            tile.Slot.Tile = tile;
            Slot = slot;
            Slot.Tile = this;

            if (adjustPosition)
            {
                Position = Slot.Position;
                tile.Position = tile.Slot.Position;
            }
        }
    }
}
