using Love;
using Match3.Animation;
using Match3.Basics;
using Match3.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.BonusEntities
{
    public class BombBonus : BasicBonusEntity
    {
        TileSlot _spawnPoint;
        int _explosionRadius;
        public BombBonus(TileSlot spawnPoint, int explosionRadius, float timeToExplode) : base()
        {
            _spawnPoint = spawnPoint;
            _explosionRadius = explosionRadius;
            Position = spawnPoint.Position;
            Animation = new FadeInAnimation(this, timeToExplode);
        }
        public override void Draw()
        {
            Graphics.SetColor(Color.Orange);
            Graphics.Circle(DrawMode.Fill, 0, 0, TileSlot.TILE_WORLD_SIZE / 2 + TileSlot.TILE_WORLD_SIZE * _explosionRadius, 30);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Animation.State == AnimationState.Finished)
            {
                foreach (var slot in _spawnPoint.GetNeighboursInRadius(_explosionRadius))
                {
                    slot.Tile?.Match();
                }
                Destroy();
            }
        }
    }
}
