using System;
using System.Collections.Generic;
using System.Text;
using Match3.Basics;
using Love;

namespace Match3.Animation
{ 
    public class FadeInAnimation : BasicAnimation
    {
        float _timeToFade;
        float _remainTime;
        public FadeInAnimation(Entity entity, float timeToFade) : base(entity)
        {
            _timeToFade = timeToFade;
            _remainTime = timeToFade;
            entity.Scale = 0f;
        }
        public override void Update(float deltaTime)
        {
            _remainTime -= deltaTime;

            Owner.Scale = Mathf.Lerp(0f, 1f, (_timeToFade - _remainTime) / _timeToFade);
            if (_remainTime <= 0f)
            {
                Finish();
            }
        }
    }
}
