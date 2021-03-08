using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Love;
using Match3.Tiles;
using Match3.Basics;
using Match3.Animation;
using Match3.Matchers;
using Match3.BonusEntities;
using Match3.Misc;

namespace Match3.Scene
{
    public enum BoardState
    {
        AwaitingInput,
        Animating,
        SpawningTiles
    }

    public class Board
    {
        public const int TILE_PIXEL_SIZE = 40;
        public const int MINIMAL_COMBO = 3;
        public BoardState State { get; protected set; } = BoardState.SpawningTiles;
        public TileSlot[,] Slots { get; protected set; }
        public int ColumnCount { get => Slots.GetUpperBound(0) + 1; }
        public int RowCount { get => Slots.Length / ColumnCount; }
        public List<Entity> Entities { get; private set; } = new List<Entity>();
        public List<IMatcher> Matchers { get; private set; } = new List<IMatcher>();
        public int Score { get; set; } = 0;
        public ITileGenerator TileGenerator { get; set; }
        TileSwapper _tileSwapper = new TileSwapper();

        public Board(int rows, int columns)
        {
            Slots = new TileSlot[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Slots[i, j] = new TileSlot(this, i, j);
                }
            }
        }

        public void Restart()
        {
            Score = 0;
            State = BoardState.SpawningTiles;
            _tileSwapper.ClearSelection();
            foreach (var e in Entities)
            {
                e.Destroy();
            }
            Entities.Clear();
        }
        public TileSlot GetSlotFromScreenPosition(Vector2 screenPos)
        {
            var worldPos = screenPos / (TILE_PIXEL_SIZE / TileSlot.TILE_WORLD_SIZE);

            foreach (var slot in Slots)
            {
                if (slot.Bounds.Contains(worldPos))
                {
                    return slot;
                }
            }
            return null;
        }
        public void Draw()
        {
            Graphics.Push(StackType.All);
            Graphics.SetScissor(0, 0, TILE_PIXEL_SIZE * ColumnCount, TILE_PIXEL_SIZE * RowCount);
            Graphics.Scale(TILE_PIXEL_SIZE / TileSlot.TILE_WORLD_SIZE);
            foreach (var entity in Entities.OrderBy(e => e.DrawPriority))
            {
                entity.DrawInternal();
            }
            Graphics.Pop();
        }
        public void Update(float deltaTime)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                var entity = Entities[i];
                entity.UpdateInternal(deltaTime);
            }

            for (int i = 0; i < Entities.Count; i++)
            {
                var entity = Entities[i];
                if (entity.Destroyed)
                {
                    Entities.Remove(entity);

                    if (entity is BasicTile)
                    {
                        Score++;
                    }
                }
            }

            NextState();
        }
        void NextState()
        {
            Func<bool> hasAnyBlockingAnimations = () => Entities.Where(e => e.Animation?.State == AnimationState.Running && e.Animation.BlocksInput).Any();
            switch (State)
            {
                case BoardState.AwaitingInput:
                    if (hasAnyBlockingAnimations())
                    {
                        State = BoardState.Animating;
                    }
                    break;
                case BoardState.Animating:
                    {
                        if (!hasAnyBlockingAnimations())
                        {
                            var matchCount = RunMatchers();
                            if (matchCount == 0)
                            {
                                _tileSwapper.SwapSelectedTiles();
                            }
                            _tileSwapper.ClearSelection();

                            if (!hasAnyBlockingAnimations())
                            {
                                State = BoardState.SpawningTiles;
                            }
                        }
                        break;
                    }
                case BoardState.SpawningTiles:
                    {
                        if (!hasAnyBlockingAnimations())
                        {
                            if (FillEmptySlots() == 0)
                            {
                                State = BoardState.AwaitingInput;
                            }
                            else
                            {
                                State = BoardState.Animating;
                            }
                        }
                        break;
                    }
            }
        }
        int FillEmptySlots()
        {
            var result = 0;
            if (TileGenerator != null)
            {
                foreach (var slot in Slots)
                {
                    if (slot.Empty)
                    {
                        SpawnTile(TileGenerator.NextTile(), slot);
                        result++;
                    }
                }
            }
            return result;
        }
        int RunMatchers()
        {
            var result = 0;

            var bonusSpawnedSlots = new List<TileSlot>();
            var countedMatches = new List<TileSlot>();
            var totalMatches = new List<List<TileSlot>>();
            foreach (var matcher in Matchers)
            {
                var matches = matcher.CalculateMatches(this, MINIMAL_COMBO);
                foreach (var match in matches)
                {
                    foreach (var slot in match)
                    {
                        if (!countedMatches.Contains(slot))
                        {
                            slot.Tile.Match();
                            result++;
                            countedMatches.Add(slot);
                        }
                    }

                    var lineMatcher = matcher as LineMatcher;
                    if (lineMatcher != null)
                    {
                        if (match.Count > 3)
                        {
                            var bonusSpawnSlot = match.Intersect(_tileSwapper.SelectedTiles.Select(t => t.Slot)).FirstOrDefault();
                            if (bonusSpawnSlot == null)
                            {
                                bonusSpawnSlot = match.LastOrDefault();
                            }

                            if (!bonusSpawnedSlots.Contains(bonusSpawnSlot))
                            {
                                if (match.Count > 4)
                                {
                                    SpawnTile(new BombTile(bonusSpawnSlot.Tile as ColoredTile), bonusSpawnSlot);
                                }
                                else
                                {
                                    SpawnTile(new LineTile(bonusSpawnSlot.Tile as ColoredTile, lineMatcher.XDir, lineMatcher.YDir), bonusSpawnSlot);
                                }
                                bonusSpawnedSlots.Add(bonusSpawnSlot);
                            }
                        }
                    }
                }

                totalMatches.AddRange(matches);
            }

            for (int i = 0; i < totalMatches.Count; i++)
            {
                for (int j = i + 1; j < totalMatches.Count; j++)
                {
                    var intersectSlot = totalMatches[i].Intersect(totalMatches[j]).FirstOrDefault();
                    if (intersectSlot != null && !bonusSpawnedSlots.Contains(intersectSlot))
                    {
                        SpawnTile(new BombTile(intersectSlot.Tile as ColoredTile), intersectSlot);
                    }
                }
            }

            return result;
        }
        public void SpawnTile(BasicTile tile, TileSlot slot)
        {
            slot.Tile?.Destroy();
            tile.AssignToSlot(slot);
            Entities.Add(tile);
        }
        public void MousePressed(float x, float y, int button)
        {
            if (State == BoardState.AwaitingInput)
            {
                var slot = GetSlotFromScreenPosition(new Vector2(x, y));

                if (slot != null)
                {
                    if (button == 0)
                    {
                        if (slot.Tile != null)
                        {
                            _tileSwapper.UserSelectTile(slot.Tile);
                        }
                    }

                    // for debug
                    else if (button == 1)
                    {
                        Entities.Add(new LineBonus(slot, 1, 0, BasicTile.DEFAULT_FALL_SPEED * 2));
                    }
                    else if (button == 2)
                    {
                        Entities.Add(new BombBonus(slot, 4, 1f));
                    }
                    else if (button == 3)
                    {
                        slot.Tile?.Match();
                    }
                }
            }
        }
    }
}
