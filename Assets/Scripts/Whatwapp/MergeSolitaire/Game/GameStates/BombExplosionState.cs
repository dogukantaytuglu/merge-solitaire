using System.Collections.Generic;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class BombExplosionState : BaseState
    {
        private readonly Board _board;
        public static List<Cell> CellsToExplode;
        public static Cell BombCell;
        public bool ExplosionComplete { get; private set; }

        private Vector2Int[] _directions = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };

        public BombExplosionState(GameController gameController, Board board, IStateAnimation stateAnimation = null) :
            base(gameController, stateAnimation)
        {
            _board = board;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            CellsToExplode = new();
            ExplosionComplete = false;
            FillCellsToExplodeBuffer();
        }

        private void FillCellsToExplodeBuffer()
        {
            if (TryGetBombCell(out BombCell) == false)
            {
                ExplosionComplete = true;
                return;
            }


            foreach (var direction in _directions)
            {
                var neighbourCell = _board.GetCell(BombCell.Coordinates + direction);
                if (neighbourCell == null) continue;
                CellsToExplode.Add(neighbourCell);
            }
            
            PlayStateAnimation();
            ExplosionComplete = true;
        }

        private bool TryGetBombCell(out Cell bombCell)
        {
            bombCell = null;
            foreach (var cell in _board.Cells)
            {
                if (cell.Block is BombBlock)
                {
                    bombCell = cell;
                }
            }

            return bombCell != null;
        }
    }
}