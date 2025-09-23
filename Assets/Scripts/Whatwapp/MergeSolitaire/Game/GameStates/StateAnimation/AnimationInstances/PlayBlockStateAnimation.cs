using DG.Tweening;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class PlayBlockStateAnimation : IStateAnimation
    {
        private readonly PlayBlockState _playBlockState;
        public bool IsAnimationActive { get; private set; }

        private Tween _animationTween;

        public PlayBlockStateAnimation(PlayBlockState playBlockState)
        {
            _playBlockState = playBlockState;
        }
        
        public void Play()
        {
            IsAnimationActive = true;
            _animationTween = _playBlockState.PlayedBlock.PlaySpawnScaleUpAnimation().OnComplete(() => IsAnimationActive = false);
        }

        public void Kill(bool complete)
        {
            _animationTween?.Kill(complete);
        }
    }
}