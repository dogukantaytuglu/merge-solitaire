using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Whatwapp.MergeSolitaire.Game.StateAnimation
{
    public class MoveBlocksStateAnimation : IStateAnimation
    {
        private readonly IReadOnlyCollection<Cell> _movingCells;
        private readonly Board _board;

        public MoveBlocksStateAnimation(IReadOnlyCollection<Cell> movingCells, Board board)
        {
            _movingCells = movingCells;
            _board = board;
        }

        public void Play(Action onComplete)
        {
            var sequence = DOTween.Sequence();
            foreach (var cell in _movingCells)
            {
                var block = cell.Block;
                var targetCell = _board.GetCell(cell.Coordinates.x, cell.Coordinates.y + 1);
                targetCell.Block = block;
                cell.Block = null;
                sequence.Join(block.Visual.MoveToPosition(targetCell.Position));
            }

            sequence.OnComplete(onComplete.Invoke);
            sequence.Play();
        }
    }
}