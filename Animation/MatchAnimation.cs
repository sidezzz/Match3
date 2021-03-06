using Love;
using Match3.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Animation
{
    public class MatchAnimation : FadeOutAnimation
    {
        public MatchAnimation(BasicTile tile, float fadeOutTime) : base(tile, fadeOutTime)
        {
        }
        public override void Finish()
        {
            base.Finish();
            Owner.Destroy();
        }
    }
}
