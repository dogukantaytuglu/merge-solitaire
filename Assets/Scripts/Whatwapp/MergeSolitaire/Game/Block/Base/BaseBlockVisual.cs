using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BaseBlockVisual : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] protected AnimationSettings _animationSettings;
        [Header("Settings")] [SerializeField] protected ColorSettings _colorSettings;

        private Tween _shakeTween;
        private Tween _movementTween;
        private Vector3 _initScale;

        private void Awake()
        {
            _initScale = transform.localScale;
        }

        public Tween MoveToPosition(Vector2 targetPos)
        {
            _movementTween?.Kill();
            return _movementTween = transform.DOMove(targetPos, _animationSettings.BlockMoveDuration);
        }

        public Tween ShakeScale()
        {
            _shakeTween?.Kill(true);
            return _shakeTween = transform.DOShakeScale(_animationSettings.BlockShakeDuration,
                    _animationSettings.BlockShakeStrength)
                .OnComplete(ResetScale);
        }
        
        public Tween PlayScaleUpAnimation()
        {
            transform.localScale = Vector3.zero;
            return transform.DOScale(Vector3.one, _animationSettings.MergeDuration).SetEase(Ease.OutBack);
        }

        protected void ResetScale()
        {
            transform.localScale = _initScale;
        }
    }
}