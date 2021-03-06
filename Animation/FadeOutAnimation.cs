using Love;
using Match3.Basics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Animation
{
    public class FadeOutAnimation : BasicAnimation
    {
        float _timeToFadeOut;
        float _remainTime;
        public FadeOutAnimation(Entity entity, float fadeOutTime) : base(entity)
        {
            _timeToFadeOut = fadeOutTime;
            _remainTime = fadeOutTime;
        }
        public override void Update(float deltaTime)
        {
            _remainTime -= deltaTime;

            Owner.Scale = Mathf.Lerp(1f, 0f, (_timeToFadeOut - _remainTime) / _timeToFadeOut);
            if (_remainTime <= 0f)
            {
                Finish();
            }
        }
    }
}
