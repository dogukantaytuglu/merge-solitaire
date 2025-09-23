using System.Collections.Generic;
using DG.Tweening;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MoveBlocksState : BaseState
    {
        public List<Cell> MovingCellsBuffer;

        private bool _canMoveBlocks;
        private Board _board;
        private readonly AnimationSettings _animationSettings;

        private int _startingRow;
        private bool _isWaitingForMovementDelay;
        
        public MoveBlocksState(GameController gameController, Board board, AnimationSettings animationSettings) : base(gameController)
        {
            _board = board;
            _animationSettings = animationSettings;
            MovingCellsBuffer = new List<Cell>();
            _stateAnimation = new MoveBlocksStateAnimation(board, this);

        }
        
        public override void OnEnter()
        {
            _isWaitingForMovementDelay = true;
            DOVirtual.DelayedCall(_animationSettings.BlockMoveDelay, () => _isWaitingForMovementDelay = false);
            base.OnEnter();
            MovingCellsBuffer.Clear();
            _startingRow = _board.Height - 2;
        }
        
        public override void OnExit()
        {
            base.OnExit();
            HasMovableBlocks();
        }
        
        public override void Update()
        {
            if (_isWaitingForMovementDelay) return;
            if (IsStateAnimationActive) return;
            if (TryFindMovableCells())
            {
                PlayStateAnimation();
            }
        }

        private bool TryFindMovableCells()
        {
            MovingCellsBuffer.Clear();
            for(var i=0; i<_board.Width; i++)
            {
                for(var j=_startingRow; j>=0; j--)
                {
                    var cell = _board.GetCell(i, j);
                    if (cell == null || cell.IsEmpty) continue;
                    
                    // Check if the block can move up
                    var upperCell = _board.GetCell(i, j + 1);
                    if (upperCell == null || !upperCell.IsEmpty) continue;
                    
                    MovingCellsBuffer.Add(cell);
                }
            }
            return MovingCellsBuffer.Count > 0;
        }
        
        public bool CanMoveBlocks()
        {
            return HasMovableBlocks();
        }
        
        private bool HasMovableBlocks()
        {
            var _startingRow = _board.Height - 2;
            for(var i=0; i<_board.Width; i++)
            {
                for(var j=_startingRow; j>=0; j--)
                {
                    var cell = _board.GetCell(i, j);
                    if (cell == null || cell.IsEmpty) continue;
                    
                    // Check if the block can move up
                    var upperCell = _board.GetCell(i, j + 1);
                    if (upperCell == null || !upperCell.IsEmpty) continue;

                    return true;
                }
            }
            return false;
        }
        
    }
}