using Match3.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Match3.Matchers
{
    public class LineMatcher : IMatcher
    {
        public int XDir { get; private set; }
        public int YDir { get; private set; }
        public LineMatcher(int xDir, int yDir)
        {
            XDir = xDir;
            YDir = yDir;
        }
        public List<List<TileSlot>> CalculateMatches(Board board, int minimalCombo)
        {
            var result = new List<List<TileSlot>>();

            foreach (var slot in board.Slots)
            {
                var chain = new List<TileSlot>();
                var curSlot = slot;
                while (curSlot != null)
                {
                    var neighbour = curSlot?.GetNeighbour(XDir, YDir);

                    chain.Add(curSlot);
                    if (!(curSlot.Tile != null && neighbour?.Tile != null && curSlot.Tile.MatchTypeName == neighbour.Tile.MatchTypeName))
                    {
                        break;
                    }

                    curSlot = neighbour;
                }

                if (chain.Count >= minimalCombo)
                {
                    result.Add(chain);
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                var chain = result[i];
                for (int j = i + 1; j < result.Count; j++)
                {
                    var nextChain = result[j];
                    if (chain.Intersect(nextChain).Any())
                    {
                        if (nextChain.Count > chain.Count)
                        {
                            result.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            result.RemoveAt(j);
                        }
                        break;
                    }
                }
            }

            return result;
        }
    }
}
