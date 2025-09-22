using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    [CreateAssetMenu(menuName = "MergeSolitaire/Settings/Game", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public float ProbabilityOfGoodBlock => _probabilityOfGoodBlock;
        public float ProbabilityToSpawnAttachableBlock => _probabilityToSpawnAttachableBlock;

        [SerializeField] [Range(0f, 1f)] private float _probabilityOfGoodBlock = 0.108f;
        [SerializeField] [Range(0f, 1f)] private float _probabilityToSpawnAttachableBlock = 0.5f;
        [SerializeField] [Range(0f, 1f)] private float _probabilityToSpawnBombBlock = 1f;
    }
}