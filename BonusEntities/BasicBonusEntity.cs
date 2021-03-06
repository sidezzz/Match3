using Match3.Basics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.BonusEntities
{
    public abstract class BasicBonusEntity : Entity
    {
        public BasicBonusEntity()
        {
            DrawPriority = DrawPriority.Bonus;
        }
    }
}
