using Love;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Tiles
{
    public class ColoredTile : BasicTile
    {
        public Color Color { get; set; }
        public ColoredTile(Color color, string matchTypeName) : base()
        {
            Color = color;
            MatchTypeName = $"{matchTypeName}{color.ToString()}";
        }
        public override void Draw()
        {
            Graphics.SetColor(Color);
        }
    }
}
