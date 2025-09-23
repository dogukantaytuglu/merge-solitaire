using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    [CreateAssetMenu(menuName = "MergeSolitaire/Settings/Animations", fileName = "AnimationSettings")]
    public class AnimationSettings : ScriptableObject
    {
        [Header("Block Spawn")]
        [SerializeField] private float _blockSpawnDuration = 0.25f;
        
        [Header("Block Move")]
        [SerializeField] private float _blockMoveDuration = 0.01f;
        [SerializeField] private float _blockMoveDelay = 0.0025f;
        
        [Header("Block Merge")]
        [SerializeField] private float _blockMergeDuration = 0.1f;
        [SerializeField] private float _blockMergeDelay = 0.2f;
        [SerializeField] private float _tremorDuration = 0.25f;
        [SerializeField] private float _tremorStrength = 0.1f;
        
        [Header("Block Shake")]
        [SerializeField] private float _blockShakeDuration = 0.25f;
        [SerializeField] private float _blockShakeStrength = 0.1f;
        
        [Header("Block To Foundation")]
        [SerializeField] private float _attachDuration = 0.35f;
        
        [Header("Cell")]
        [SerializeField] private float _highlightDuration = 0.05f;
        [SerializeField] private float _highlightDelay = 0.03f;

        [Header("Bomb Block")] 
        [SerializeField] private float _explosionDelay = 1f;
        [SerializeField] private float _bombInflateAmount = 1.3f;
        [SerializeField] private float _bombExplodeDuration = 1f;
        [SerializeField] private float _bombShakeStrength = 90f;
        [SerializeField] private float _cameraShakeAmplitude = 5f;
        [SerializeField] private float _cameraShakeFrequency = 2.5f;
        [SerializeField] private float _cameraShakeDuration = 0.85f;

        [Header("Score Animation")] 
        [SerializeField] private float _scoreAnimationDuration = 0.2f;
        [SerializeField] private float _scoreAnimationPower = 1.1f;

        [Header("Floater")] 
        [SerializeField] private float _scoreFloaterYMoveAmount = 1.5f;
        [SerializeField] private float _scoreFloaterMoveDuration = 1f;
        
        
        public float BlockMoveDuration => _blockMoveDuration;
        public float BlockMoveDelay => _blockMoveDelay;
        public float MergeDuration => _blockMergeDuration;
        public float TremorDuration => _tremorDuration;
        public float TremorStrength => _tremorStrength;
        public float SpawnDuration => _blockSpawnDuration;
        public float AttachDuration => _attachDuration;
        public float HighlightDelay => _highlightDelay;
        public float HighlightDuration => _highlightDuration;
        public float BlockShakeDuration => _blockShakeDuration;
        public float BlockShakeStrength => _blockShakeStrength;
        public float BlockMergeDelay => _blockMergeDelay;
        public float BombInflateAmount => _bombInflateAmount;
        public float BombExplodeDuration => _bombExplodeDuration;
        public float BombShakeStrength => _bombShakeStrength;
        public float CameraShakeAmplitude => _cameraShakeAmplitude;
        public float CameraShakeFrequency => _cameraShakeFrequency;
        public float CameraShakeDuration => _cameraShakeDuration;
        public float ScoreAnimationDuration => _scoreAnimationDuration;
        public float ScoreAnimationPower => _scoreAnimationPower;
        public float ScoreFloaterYMoveAmount => _scoreFloaterYMoveAmount;
        public float ScoreFloaterMoveDuration => _scoreFloaterMoveDuration;
    }
}