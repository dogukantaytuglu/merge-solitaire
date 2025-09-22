using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public abstract class BaseBlock : MonoBehaviour
    {
        public abstract Tween MoveToPosition(Vector2 targetPos);
        public abstract Tween ShakeScale();
        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}