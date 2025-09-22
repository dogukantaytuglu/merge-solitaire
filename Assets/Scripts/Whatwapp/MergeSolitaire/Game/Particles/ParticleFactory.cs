using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.Particles
{
    public class ParticleFactory : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionParticle;

        public ParticleSystem CreateExplosionParticle()
        {
            var particle = Instantiate(_explosionParticle, transform);
            return particle;
        }
    }
}