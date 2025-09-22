using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlock : BaseBlock
    {
        [SerializeField] private BombBlockVisual _visual;
        
        public void Init()
        {
        }

        public override Tween MoveToPosition(Vector2 targetPos)
        {
            return _visual.MoveToPosition(targetPos);
        }

        public override Tween ShakeScale()
        {
           return _visual.ShakeScale();
        }
    }
}