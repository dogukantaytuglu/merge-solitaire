using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class Cell : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CellVisual _cellVisual;
        
        public Vector2 Position => transform.position;
        public Vector2Int Coordinates { get; private set; }
        public bool IsEmpty => _block == null;

        public BaseBlock Block
        {
            get => _block;

            set
            {
                if (value == null)
                {
                    _block = null;
                    return;
                }


                if (_block != null)
                {
                    Debug.LogException(new System.Exception($"Block already exists in cell {Coordinates}"));
                    _block.Remove();
                }

                _block = value;
                _block.transform.SetParent(transform);
            }
        }

        private BaseBlock _block;


        public void Init(Vector2Int coordinates)
        {
            Coordinates = coordinates;
            gameObject.name = $"Cell ({coordinates.x}, {coordinates.y})";
            _cellVisual.Init(coordinates);
        }
        
        public void OnClick()
        {
            _cellVisual.Highlight();
        }
    }
}