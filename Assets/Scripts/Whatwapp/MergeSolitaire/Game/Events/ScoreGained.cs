using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.Events
{
    public struct ScoreGained : IEvent
    {
        public readonly int Amount;
        public readonly Vector2? FloaterPosition;

        public ScoreGained(int amount, Vector2? floaterPosition = null)
        {
            Amount = amount;
            FloaterPosition = floaterPosition;
        }
    }
}