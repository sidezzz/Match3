using Match3.Animation;
using Match3.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Misc
{
    public class TileSwapper
    {
        List<BasicTile> _selectedTiles = new List<BasicTile>();
        public List<BasicTile> SelectedTiles => new List<BasicTile>(_selectedTiles);
        public TileSwapper()
        {
        }
        public void UserSelectTile(BasicTile tile)
        {
            if (_selectedTiles.Contains(tile))
            {
                tile.StopSelectAnimation();
                _selectedTiles.Remove(tile);
            }
            else if (_selectedTiles.Count == 0)
            {
                _selectedTiles.Add(tile);
                tile.StartSelectAnimation();
            }
            else if (_selectedTiles.Count == 1 && _selectedTiles[0].Slot.GetNeighboursInRadius(1).Contains(tile.Slot))
            {
                _selectedTiles.Add(tile);
                tile.StartSelectAnimation();
                SwapSelectedTiles();
            }
            else
            {
                ClearSelection();
            }
        }
        public void SwapSelectedTiles()
        {
            if (_selectedTiles.Count == 2)
            {
                _selectedTiles[0].SwapWith(_selectedTiles[1], false);

                _selectedTiles[0].Animation = new MoveToSlotAnimation(_selectedTiles[0], _selectedTiles[0].Slot, 5f);
                _selectedTiles[1].Animation = new MoveToSlotAnimation(_selectedTiles[1], _selectedTiles[1].Slot, 5f);
            }
        }
        public void ClearSelection()
        {
            foreach (var tile in _selectedTiles)
            {
                tile.StopSelectAnimation();
            }
            _selectedTiles.Clear();
        }
    }
}
