using UnityEngine;
using Whatwapp.Core.FSM;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public abstract class BaseState : IState
    {
        public bool IsStateAnimationActive => _stateAnimation?.IsAnimationActive ?? false;
        protected GameController _gameController;
        protected IStateAnimation _stateAnimation;

        protected BaseState(GameController gameController)
        {
            _gameController = gameController;
        }

        public virtual void OnEnter()
        {
            Debug.Log("Entering state: " + GetType().Name);
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
            _stateAnimation?.Kill(true);
        }

        protected void PlayStateAnimation()
        {
            _stateAnimation?.Play();
        }
    }
}