using System;

namespace Whatwapp.MergeSolitaire.Game.StateAnimation
{
    public interface IStateAnimation
    {
        void Play(Action onComplete);
    }
}