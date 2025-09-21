using System.Collections.Generic;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MoveBlocksState : BaseState
    {
        
        private bool _isMovingBlocks;
        private bool _canMoveBlocks;
        private Board _board;
        private BlockGroupAnimationController _blockGroupAnimationController;

        private List<Cell> _movingCells;
        private int _startingRow;
        
        public MoveBlocksState(GameController gameController, Board board, BlockGroupAnimationController blockGroupAnimationController) : base(gameController)
        {
            _board = board;
            _blockGroupAnimationController = blockGroupAnimationController;
            _movingCells = new List<Cell>();
        }

        
        public override void OnEnter()
        {
            base.OnEnter();
            _movingCells.Clear();
            _isMovingBlocks = false;
            _startingRow = _board.Height - 2;
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _isMovingBlocks = false;
            HasMovableBlocks();
        }
        
        public override void Update()
        {
            if (_isMovingBlocks) return;
            _isMovingBlocks = true;
            if (FindMovableCells())
            {
                MoveBlocks();
            }
            else
            {
                _isMovingBlocks = false;
            }
        }

        private bool FindMovableCells()
        {
            _movingCells.Clear();
            for(var i=0; i<_board.Width; i++)
            {
                for(var j=_startingRow; j>=0; j--)
                {
                    var cell = _board.GetCell(i, j);
                    if (cell == null || cell.IsEmpty) continue;
                    
                    // Check if the block can move up
                    var upperCell = _board.GetCell(i, j + 1);
                    if (upperCell == null || !upperCell.IsEmpty) continue;
                    
                    _movingCells.Add(cell);
                }
            }
            return _movingCells.Count > 0;
        }

        private void MoveBlocks()
        {
            _blockGroupAnimationController.PlayMoveBlocksAnimation(_movingCells, () => _isMovingBlocks = false);
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