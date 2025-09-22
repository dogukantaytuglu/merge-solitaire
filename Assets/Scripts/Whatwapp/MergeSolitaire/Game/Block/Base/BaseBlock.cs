using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public abstract class BaseBlock : MonoBehaviour
    {
        public abstract Sequence MoveToPosition(Vector2 targetPos);
        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}