using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Whatwapp.Core.Utils;

namespace Whatwapp.MergeSolitaire.Game
{
    public class NextBlockController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BlockFactory _blockFactory;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Board _board;
        [SerializeField] private FoundationsController _foundationsController;
        
        [Header("Settings")]
        [SerializeField] private AnimationSettings _animationSettings;
        [SerializeField] private GameSettings _gameSettings;
        
        public bool IsReady => _nextBlock != null && _isReady;
        public bool HasBlock => _nextBlock != null;
        
        private BaseBlock _nextBlock;
        private bool _isReady;
        
        
        public void ExtractNextBlock()
        {
            if (_nextBlock != null) return;

            if (Random.value < _gameSettings.ProbabilityToSpawnBombBlock)
            {
                CreateBombBlock();
            }

            else
            {
                CreateMergeBlock();
            }
            
            
            _nextBlock.transform.SetParent(_spawnPoint);
            _isReady = false;
            AnimateSpawn();
        }

        private void CreateBombBlock()
        {
            _nextBlock = _blockFactory.CreateBombBlock();
        }

        private void CreateMergeBlock()
        {
            var seed = EnumUtils.GetRandom<BlockSeed>();
            var value = EnumUtils.GetRandom<BlockValue>(BlockValue.Ace, BlockValue.King);

            if (Random.value < _gameSettings.ProbabilityOfGoodBlock)
            {
                if (Random.value < _gameSettings.ProbabilityToSpawnAttachableBlock)
                {
                    value = ExtractAttachableBlock(value);
                }
                else
                {
                    var nextBlocks = _foundationsController.GetNextBlocks();
                    if (nextBlocks.Count > 0)
                    {
                        var item = nextBlocks[Random.Range(0, nextBlocks.Count)];
                        value = item.Item2;
                        seed = item.Item1;
                    }
                }
            }
                
            _nextBlock = _blockFactory.CreateMergeBlock(value, seed);
        }

        private BlockValue ExtractAttachableBlock(BlockValue value)
        {
            var attachableBlocks = _board.GetAttachableBlocks();
            if (attachableBlocks.Count <= 0) return value;
            var block = attachableBlocks[Random.Range(0, attachableBlocks.Count)]; 
            value = block.Value;
            return value;
        }

        public BaseBlock PopBlock()
        {
            var block = _nextBlock;
            _nextBlock = null;
            return block;
        }

        private void AnimateSpawn()
        {
            _nextBlock.transform.localScale = Vector3.zero;
            _nextBlock.transform.localPosition = Vector3.zero;
            _nextBlock.transform.DOScale(Vector3.one, _animationSettings.SpawnDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
            {
                _isReady = true;
            });
        }
    }
}