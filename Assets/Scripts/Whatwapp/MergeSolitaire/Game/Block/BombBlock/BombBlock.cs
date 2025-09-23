using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlock : BaseBlock
    {
        [SerializeField] private BombBlockVisual _visual;
        
        public void Init()
        {
            _visual.EnableTrail(false);
        }

        public override Tween MoveToPosition(Vector2 targetPos)
        {
            return _visual.MoveToPosition(targetPos).OnComplete(() => _visual.EnableTrail(false));
        }

        public override Tween ShakeScale()
        {
           return _visual.ShakeScale();
        }

        public override Tween PlaySpawnScaleUpAnimation()
        {
            return _visual.PlaySpawnScaleUpAnimation();
        }

        public override void PutBlockInCell(Cell cell)
        {
            base.PutBlockInCell(cell);
            _visual.EnableTrail(true);
        }

        public override Sequence Explode()
        {
            return _visual.Explode(Remove);
        }
    }
}