using System;
using System.Collections.Generic;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksState : BaseState
    {
        private readonly Board _board;

        public bool MergeCompleted { get; private set; }
        public int MergeCount { get; private set; }

        private IStateAnimation _mergeBlocksAnimation;
        private List<List<Cell>> _mergeableGroupsBuffer;

        private Vector2Int[] _directions = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };


        public MergeBlocksState(GameController gameController, Board board, FoundationsController foundationsController, BlockFactory blockFactory) : base(gameController)
        {
            _board = board;
            _mergeableGroupsBuffer = new();
            _mergeBlocksAnimation = new MergeBlocksStateAnimation(gameController, foundationsController, blockFactory, _mergeableGroupsBuffer);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            MergeCompleted = false;
            MergeCount = 0;
            MergeBlocks();
        }

        public override void OnExit()
        {
            base.OnExit();
            _mergeBlocksAnimation?.Kill(true);
        }

        private void MergeBlocks()
        {
            MergeCompleted = false;
            // Check for all cells that are not empty 
            if (TryFillMergeableGroupsBuffer(_mergeableGroupsBuffer))
            {
                MergeCount = _mergeableGroupsBuffer.Count;
                PlayMergeAnimation(OnMergeAnimationComplete);
            }
            else
            {
                MergeCompleted = true;
            }
        }
        
        private void PlayMergeAnimation(Action onComplete)
        {
            _mergeBlocksAnimation.Play(onComplete);
        }

        private void OnMergeAnimationComplete()
        {
            MergeCompleted = true;
        }

        private bool TryFillMergeableGroupsBuffer(List<List<Cell>> buffer)
        {
            buffer.Clear();
            var visited = new bool[_board.Width, _board.Height];
            for(var x= 0; x < _board.Width; x++)
            {
                for(var y = _board.Height-1; y>=0; y--)
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
                        (nextCell.Block is MergeBlock nextMergeBlock && nextMergeBlock.Value != value)) continue;
                    visited[nextCell.Coordinates.x, nextCell.Coordinates.y] = true;
                    queue.Enqueue(nextCell);
                }
            }
            
            return mergeableCells;
        }
    }
}