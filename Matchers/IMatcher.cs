using System;
using System.Collections.Generic;
using System.Text;
using Match3.Scene;

namespace Match3.Matchers
{
    public interface IMatcher
    {
        List<List<TileSlot>> CalculateMatches(Board board, int minimalCombo);
    }
}
