using DG.Tweening;
using UnityEngine;
using TMPro;
using Whatwapp.Core.Audio;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockVisual : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private ColorSettings _colorSettings;
        [SerializeField] private AnimationSettings _animationSettings;

        [Header("References")] [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private TextMeshPro _text;


        private Vector3 _defaultScale;

        public void Init(BlockValue value, BlockSeed seed)
        {
            _defaultScale = transform.localScale;
            _spriteRenderer.sprite = _colorSettings.GetBlockSprite(seed);
            _text.text = value.Symbol();
        }

        private void ShakeScale()
        {
            transform.DOShakeScale(_animationSettings.BlockShakeDuration, _animationSettings.BlockShakeStrength)
                .OnComplete(() => transform.localScale = _defaultScale);
        }

        public Tween PlayTremorAnimation()
        {
            return transform.DOShakeScale(_animationSettings.TremorDuration,
                _animationSettings.TremorStrength);
        }

        public Sequence MoveToPosition(Vector2 targetPos)
        {
            return DOTween.Sequence()
                .AppendInterval(_animationSettings.BlockMoveDelay)
                .Append(transform.DOMove(targetPos, _animationSettings.BlockMoveDuration))
                .OnComplete(ShakeScale);
        }

        private Tween MoveToMergePosition(Vector2 targetPos)
        {
            return transform.DOMove(targetPos, _animationSettings.MergeDuration);
        }

        public Tween PlayScaleDownAnimation()
        {
            return transform.DOScale(Vector3.zero, _animationSettings.MergeDuration);
        }
        
        public Sequence PlayMergeAnimation(Vector3 targetPos)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(MoveToMergePosition(targetPos));
            sequence.Join(PlayScaleDownAnimation());
            sequence.JoinCallback(() => { SFXManager.Instance.PlayOneShot(Consts.SFX_PlayBlock); });
            sequence.SetDelay(_animationSettings.MergeDuration);
            return sequence;
        }

        public void PlayScaleUpAnimation()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _animationSettings.MergeDuration).SetEase(Ease.OutBack);
        }
    }
}