using System;
using System.Collections.Generic;
using System.Text;
using Match3.Basics;
using Love;
using Match3.Tiles;

namespace Match3.Animation
{
    public class SelectionAnimation : BasicAnimation
    {
        float _selectedTime = 0f;
        public SelectionAnimation(BasicTile tile) : base(tile)
        {
            BlocksInput = false;
        }
        public override void Update(float deltaTime)
        {
            _selectedTime += deltaTime;
            Owner.Scale = Mathf.Cos(_selectedTime * 5f);
        }
        public override void Finish()
        {
            base.Finish();
            Owner.Scale = 1f;
        }
    }
}
