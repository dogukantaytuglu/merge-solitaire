using System;
using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlockVisual : BaseBlockVisual
    {
        [SerializeField] private ParticleSystem _trailParticle;

        private Sequence _explosionSequence;

        public void EnableTrail(bool value)
        {
            _trailParticle.gameObject.SetActive(value);
        }

        public Sequence Explode(Action remove)
        {
            _explosionSequence?.Kill(true);
            return _explosionSequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one * _animationSettings.BombInflateAmount,
                    _animationSettings.BombExplodeDuration))
                .Join(transform.DOShakeRotation(_animationSettings.BombExplodeDuration, _animationSettings.BombShakeStrength))
                .OnComplete(remove.Invoke);
        }
    }
}