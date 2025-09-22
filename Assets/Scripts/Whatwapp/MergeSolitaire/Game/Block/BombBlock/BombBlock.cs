using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlock : BaseBlock
    {
        [SerializeField] private BombBlockVisual _visual;

        public override Sequence MoveToPosition(Vector2 targetPos)
        {
            return _visual.MoveToPosition(targetPos);
        }

        public void Init()
        {
        }
    }
}