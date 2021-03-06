using Match3.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Misc
{
    public interface ITileGenerator
    {
        BasicTile NextTile();
    }
}
