using DG.Tweening;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MoveBlocksStateAnimation : IStateAnimation
    {
        public bool IsAnimationActive { get; private set; }
        private readonly Board _board;
        private readonly MoveBlocksState _moveBlocksState;

        private Sequence _sequence;

        public MoveBlocksStateAnimation(Board board, MoveBlocksState moveBlocksState)
        {
            _board = board;
            _moveBlocksState = moveBlocksState;
        }

        public void Play()
        {
            IsAnimationActive = true;
            _sequence = DOTween.Sequence();
            foreach (var cell in _moveBlocksState.MovingCellsBuffer)
            {
                var block = cell.Block;
                var targetCell = _board.GetCell(cell.Coordinates.x, cell.Coordinates.y + 1);
                targetCell.Block = block;
                cell.Block = null;
                var upperCell = _board.GetCell(cell.Coordinates.x, cell.Coordinates.y + 1);
                var sequence = DOTween.Sequence();
                sequence.Append(block.MoveToPosition(targetCell.Position));
                if (upperCell == null || upperCell.IsEmpty == false)
                {
                    sequence.Append(block.ShakeScale());
                }
            }

            _sequence.OnComplete(() => IsAnimationActive = false);
            _sequence.Play();
        }

        public void Kill(bool complete)
        {
            _sequence?.Kill(complete);
        }
    }
}