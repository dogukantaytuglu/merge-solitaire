using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlock : MonoBehaviour
    {
        [SerializeField] private BombBlockVisual _visual;

        public void Init()
        {
            _visual.Init();
        }
    }
}