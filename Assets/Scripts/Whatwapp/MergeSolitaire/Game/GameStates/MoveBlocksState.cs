using System.Collections.Generic;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MoveBlocksState : BaseState
    {
        private bool _isMovingBlocks;
        private bool _canMoveBlocks;
        private Board _board;

        private List<Cell> _movingCellsBuffer;
        private int _startingRow;
        private IStateAnimation _stateAnimation;
        
        public MoveBlocksState(GameController gameController, Board board) : base(gameController)
        {
            _board = board;
            _movingCellsBuffer = new List<Cell>();
            _stateAnimation = new MoveBlocksStateAnimation(_movingCellsBuffer, _board);
        }

        
        public override void OnEnter()
        {
            base.OnEnter();
            _movingCellsBuffer.Clear();
            _isMovingBlocks = false;
            _startingRow = _board.Height - 2;
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _stateAnimation.Kill(true);
            _isMovingBlocks = false;
            HasMovableBlocks();
        }
        
        public override void Update()
        {
            if (_isMovingBlocks) return;
            _isMovingBlocks = true;
            if (TryFindMovableCells())
            {
                MoveBlocks();
            }
            else
            {
                _isMovingBlocks = false;
            }
        }

        private bool TryFindMovableCells()
        {
            _movingCellsBuffer.Clear();
            for(var i=0; i<_board.Width; i++)
            {
                for(var j=_startingRow; j>=0; j--)
                {
                    var cell = _board.GetCell(i, j);
                    if (cell == null || cell.IsEmpty) continue;
                    
                    // Check if the block can move up
                    var upperCell = _board.GetCell(i, j + 1);
                    if (upperCell == null || !upperCell.IsEmpty) continue;
                    
                    _movingCellsBuffer.Add(cell);
                }
            }
            return _movingCellsBuffer.Count > 0;
        }

        private void MoveBlocks()
        {
            _stateAnimation.Play(OnMovementComplete);
        }

        private void OnMovementComplete()
        {
            _isMovingBlocks = false;
        }

        public bool CanMoveBlocks()
        {
            return _isMovingBlocks || HasMovableBlocks();
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