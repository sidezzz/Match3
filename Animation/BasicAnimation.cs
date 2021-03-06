using System;
using System.Collections.Generic;
using System.Text;
using Match3.Basics;

namespace Match3.Animation
{
    public enum AnimationState
    {
        Running,
        Stopped,
        Finished
    }

    public abstract class BasicAnimation
    {
        public Entity Owner { get; private set; }
        public AnimationState State { get; protected set; } = AnimationState.Running;
        public bool BlocksInput { get; protected set; } = true;
        public BasicAnimation(Entity entity)
        {
            Owner = entity;
        }
        public abstract void Update(float deltaTime);
        public virtual void Pause()
        {
            if (State == AnimationState.Running)
            {
                State = AnimationState.Stopped;
            }
        }
        public virtual void Resume()
        {
            if (State == AnimationState.Stopped)
            {
                State = AnimationState.Running;
            }
        }
        public virtual void Finish()
        {
            State = AnimationState.Finished;
        }
    }
}
