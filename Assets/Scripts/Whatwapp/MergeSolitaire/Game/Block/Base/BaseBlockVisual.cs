using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BaseBlockVisual : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected AnimationSettings _animationSettings;
        [Header("Settings")] 
        [SerializeField] protected ColorSettings _colorSettings;

        public Sequence MoveToPosition(Vector2 targetPos)
        {
            return DOTween.Sequence()
                .AppendInterval(_animationSettings.BlockMoveDelay)
                .Append(transform.DOMove(targetPos, _animationSettings.BlockMoveDuration))
                .OnComplete(ShakeScale);
        }
        
        private void ShakeScale()
        {
            var initScale = transform.localScale;
            transform.DOShakeScale(_animationSettings.BlockShakeDuration, _animationSettings.BlockShakeStrength)
                .OnComplete(() => transform.localScale = initScale);
        }
    }
}