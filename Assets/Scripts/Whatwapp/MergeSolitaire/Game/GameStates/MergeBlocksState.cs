using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksState : BaseState
    {
        private readonly Board _board;
        private readonly AnimationSettings _animationSettings;

        public bool MergeCompleted { get; private set; }
        public int MergeCount { get; private set; }

        public static List<List<Cell>> MergeableGroupsBuffer;

        private Vector2Int[] _directions = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };


        public MergeBlocksState(GameController gameController, Board board, AnimationSettings animationSettings,
            FoundationsController foundationsController, BlockFactory blockFactory) : base(
            gameController)
        {
            _board = board;
            _animationSettings = animationSettings;
            MergeableGroupsBuffer = new();
            _stateAnimation =
                new MergeBlocksStateAnimation(gameController, foundationsController, blockFactory);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            MergeCompleted = false;
            MergeCount = 0;
            DOVirtual.DelayedCall(_animationSettings.BlockMergeDelay, MergeBlocks);
        }

        private void MergeBlocks()
        {
            MergeCompleted = false;
            // Check for all cells that are not empty 
            if (TryFillMergeableGroupsBuffer(MergeableGroupsBuffer))
            {
                MergeCount = MergeableGroupsBuffer.Count;
                PlayStateAnimation();
            }

            MergeCompleted = true;
        }

        private bool TryFillMergeableGroupsBuffer(List<List<Cell>> buffer)
        {
            buffer.Clear();
            var visited = new bool[_board.Width, _board.Height];
            for (var x = 0; x < _board.Width; x++)
            {
                for (var y = _board.Height - 1; y >= 0; y--)
                {
                    if (visited[x, y]) continue;
                    var cell = _board.GetCell(x, y);
                    if (cell.IsEmpty) continue;
                    var group = GetMergeableCells(cell, visited);
                    if (group.Count > 1)
                    {
                        buffer.Add(group);
                    }
                }
            }

            return buffer.Count > 0;
        }

        private List<Cell> GetMergeableCells(Cell cell, bool[,] visited)
        {
            var mergeableCells = new List<Cell>();
            if (cell.Block is not MergeBlock mergeBlock) return mergeableCells;
            var value = mergeBlock.Value;
            var queue = new Queue<Cell>();
            queue.Enqueue(cell);
            visited[cell.Coordinates.x, cell.Coordinates.y] = true;

            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();
                mergeableCells.Add(currentCell);
                foreach (var direction in _directions)
                {
                    var nextCell = _board.GetCell(currentCell.Coordinates + direction);
                    if (nextCell == null || nextCell.IsEmpty ||
                        visited[nextCell.Coordinates.x, nextCell.Coordinates.y] ||
                        nextCell.Block is not MergeBlock nextMergeBlock || nextMergeBlock.Value != value) continue;
                    visited[nextCell.Coordinates.x, nextCell.Coordinates.y] = true;
                    queue.Enqueue(nextCell);
                }
            }

            return mergeableCells;
        }
    }
}