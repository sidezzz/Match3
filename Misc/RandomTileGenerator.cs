using Match3.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using Love;

namespace Match3.Misc
{
    public class RandomTileGenerator : ITileGenerator
    {
        Random _random = new Random();
        public BasicTile NextTile()
        {
            BasicTile tile = null;
            switch (_random.Next(0, 6))
            {
                case 0:
                    tile = new CircleTile(Color.Red);
                    break;
                case 1:
                    tile = new RectangleTile(Color.Green);
                    break;
                case 2:
                    tile = new CircleTile(Color.Yellow);
                    break;
                case 3:
                    tile = new RectangleTile(Color.Cyan);
                    break;
                case 4:
                    tile = new CircleTile(Color.Violet);
                    break;
                case 5:
                    tile = new RectangleTile(Color.Blue);
                    break;
            }

            return tile;
        }
    }
}
