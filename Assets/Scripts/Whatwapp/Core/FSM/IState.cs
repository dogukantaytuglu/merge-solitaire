namespace Whatwapp.Core.FSM
{
    public interface IState
    {
        public bool IsStateAnimationActive { get; }
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}