using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class MergeBlock : BaseBlock
    {
        [SerializeField] private MergeBlockVisual _visual;

        private BlockValue _value;
        private BlockSeed _seed;

        public BlockValue Value => _value;
        public BlockSeed Seed => _seed;

        public void Init(BlockValue value, BlockSeed seed)
        {
            _value = value;
            _seed = seed;
            _visual.Init(value, seed);
        }

        public override Tween MoveToPosition(Vector2 targetPos)
        {
            return _visual.MoveToPosition(targetPos);
        }

        public override Tween ShakeScale()
        {
            return _visual.ShakeScale();
        }

        public Tween PlayScaleDownAnimation()
        {
            return _visual.PlayScaleDownAnimation();
        }

        public Tween PlayTremorAnimation()
        {
            return _visual.PlayTremorAnimation();
        }

        public Sequence PlayMergeAnimation(Vector3 targetPos)
        {
            return _visual.PlayMergeAnimation(targetPos);
        }

        public void PlayScaleUpAnimation()
        {
            _visual.PlayScaleUpAnimation();
        }
    }
}