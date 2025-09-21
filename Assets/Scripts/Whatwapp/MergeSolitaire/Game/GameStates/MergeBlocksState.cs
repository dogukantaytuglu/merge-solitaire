using System.Collections.Generic;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksState : BaseState
    {
        private readonly Board _board;
        private readonly BlockGroupAnimationController _blockGroupAnimationController;

        public bool MergeCompleted { get; private set; }
        public int MergeCount { get; private set; }


        private Vector2Int[] _directions = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };


        public MergeBlocksState(GameController gameController, Board board, BlockGroupAnimationController blockGroupAnimationController) : base(gameController)
        {
            _board = board;
            _blockGroupAnimationController = blockGroupAnimationController;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            MergeCompleted = false;
            MergeCount = 0;
            MergeBlocks();
        }

        private void MergeBlocks()
        {
            MergeCompleted = false;
            // Check for all cells that are not empty 
            var mergeableGroups = GetAllMergableCells();
            MergeCount = mergeableGroups.Count;
            if (MergeCount == 0)
            {
                MergeCompleted = true;
                return;
            }
            MergeGroups(mergeableGroups);
        }
        
        private void MergeGroups(List<List<Cell>> mergeableGroups)
        {
           _blockGroupAnimationController.PlayMergeAnimation(mergeableGroups, () => MergeCompleted = true);
        }

        private List<List<Cell>> GetAllMergableCells()
        {
            var mergedGroups = new List<List<Cell>>();
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
                        mergedGroups.Add(group);
                    }
                }
            }
            
            return mergedGroups;
        }

        private List<Cell> GetMergeableCells(Cell cell, bool[,] visited)
        {
            var mergeableCells = new List<Cell>();
            var value = cell.Block.Value;
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
                        nextCell.Block.Value != value) continue;
                    visited[nextCell.Coordinates.x, nextCell.Coordinates.y] = true;
                    queue.Enqueue(nextCell);
                }
            }
            
            return mergeableCells;
        }
    }
}