using UnityEngine;
using Whatwapp.Core.Utils;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockFactory : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private MergeBlock _blockPrefab;
        [Header("Prefabs")]
        [SerializeField] private BombBlock _bombBlockPrefab;


        public MergeBlock CreateMergeBlock(BlockValue value, BlockSeed seed)
        {
            var block = Instantiate(_blockPrefab, this.transform);
            block.Init(value, seed);
            return block;
        }

        public BombBlock CreateBombBlock()
        {
            var bombBlock = Instantiate(_bombBlockPrefab, transform);
            bombBlock.Init();
            return bombBlock;
        }

        public MergeBlock CreateStartingBlock()
        {
            var value = EnumUtils.GetRandom<BlockValue>(BlockValue.Ace, BlockValue.King);
            var seed = EnumUtils.GetRandom<BlockSeed>();
            return CreateMergeBlock(value, seed);
        }
    }
}