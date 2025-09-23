using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public abstract class BaseBlock : MonoBehaviour
    {
        public abstract Tween MoveToPosition(Vector2 targetPos);
        public abstract Tween ShakeScale();
        public abstract Sequence Explode();
        public abstract Tween PlayScaleUpAnimation();
        public virtual void PutBlockInCell(Cell cell)
        {
            cell.Block = this;
            transform.position = cell.Position;
        }
        
        public void Remove()
        {
            Destroy(gameObject);
        }

    }
}