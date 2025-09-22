using DG.Tweening;
using Whatwapp.Core.Audio;
using Whatwapp.MergeSolitaire.Game.Particles;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class BombExplosionStateAnimation : IStateAnimation
    {
        private readonly ParticleFactory _particleFactory;
        private Sequence _sequence;

        public BombExplosionStateAnimation(ParticleFactory particleFactory)
        {
            _particleFactory = particleFactory;
        }

        public bool IsAnimationActive { get; private set; }

        public void Play()
        {
            IsAnimationActive = true;
            var cellsToExplode = BombExplosionState.CellsToExplode;

            _sequence = DOTween.Sequence();
            var explosionParticle = _particleFactory.CreateExplosionParticle();
            explosionParticle.transform.position = BombExplosionState.BombCell.Position;

            _sequence.Append(BombExplosionState.BombCell.Block.Explode());
            
            foreach (var cell in cellsToExplode)
            {
                var block = cell.Block;
                if (block == null) continue; 
                _sequence.Join(block.Explode());
            }

            _sequence.AppendCallback(() => explosionParticle.Play());
            _sequence.JoinCallback(() => SFXManager.Instance.PlayOneShot("Explosion"));

            _sequence.OnComplete(() =>
            {
                foreach (var cell in cellsToExplode)
                {
                    cell.Block = null;
                }

                BombExplosionState.BombCell.Block = null;
                
                IsAnimationActive = false;
            });
        }

        public void Kill(bool complete)
        {
            _sequence?.Kill(complete);
        }

    }
}