using System;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public interface IStateAnimation
    {
        void Play(Action onComplete);
        void Kill(bool complete);
    }
}