using System;
using System.Collections.Generic;
using System.Text;
using Love;
using Match3.Animation;

namespace Match3.Basics
{
    public enum DrawPriority
    {
        Default,
        Bonus,
        Selected
    }

    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; } = 1f;
        public float Rotation { get; set; } = 0f;
        public DrawPriority DrawPriority { get; set; } = DrawPriority.Default;
        public bool Destroyed { get; private set; } = false;
        private BasicAnimation _animation;
        public BasicAnimation Animation 
        {
            get => _animation;
            set
            {
                if (_animation != null && _animation.State != AnimationState.Finished)
                {
                    _animation.Finish();
                }

                _animation = value;
            }
        }
        public virtual void Update(float deltaTime)
        {
            //if (Animation?.State == AnimationState.Running)
            //{
            //    Animation?.Update(deltaTime);
            //}
        }
        public abstract void Draw();
        public virtual void Destroy()
        {
            Destroyed = true;
            Animation = null;
        }
        public void UpdateInternal(float deltaTime)
        {
            if (!Destroyed)
            {
                if (Animation?.State == AnimationState.Running)
                {
                    Animation?.Update(deltaTime);
                }
            }

            if (!Destroyed)
            {
                Update(deltaTime);
            }
        }
        public void DrawInternal()
        {
            if (!Destroyed)
            {
                Graphics.Push();
                Graphics.Translate(Position);
                Graphics.Rotate(Rotation);
                Graphics.Scale(Scale);
                Draw();
                Graphics.Pop();
            }
        }
    }
}
