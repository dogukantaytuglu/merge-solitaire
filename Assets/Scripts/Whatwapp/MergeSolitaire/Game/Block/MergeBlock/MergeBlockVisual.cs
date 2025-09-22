using DG.Tweening;
using UnityEngine;
using TMPro;
using Whatwapp.Core.Audio;

namespace Whatwapp.MergeSolitaire.Game
{
    public class MergeBlockVisual : BaseBlockVisual
    {
        [SerializeField] private TextMeshPro _text;
        
        [Header("References")] 
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        
        public void Init(BlockValue value, BlockSeed seed)
        {
            _defaultScale = transform.localScale;
            _spriteRenderer.sprite = _colorSettings.GetBlockSprite(seed);
            _text.text = value.Symbol();
        }
        
        public Tween PlayScaleDownAnimation()
        {
            return transform.DOScale(Vector3.zero, _animationSettings.MergeDuration);
        }
        
        public Tween PlayTremorAnimation()
        {
            return transform.DOShakeScale(_animationSettings.TremorDuration,
                _animationSettings.TremorStrength);
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
        
        private Tween MoveToMergePosition(Vector2 targetPos)
        {
            return transform.DOMove(targetPos, _animationSettings.MergeDuration);
        }
    }
}