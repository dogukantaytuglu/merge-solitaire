
namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public interface IStateAnimation
    {
        void Play();
        void Kill(bool complete);
        public bool IsAnimationActive { get; }
    }
}