using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.ScoreFloater
{
    public class FloatersController : MonoBehaviour
    {
        [SerializeField] private FloaterController floaterPrefab;

        public void ShowFloater(string message, Vector2 position)
        {
            var floater = Instantiate(floaterPrefab, transform);
            floater.Show(message, position);
        }
    }
}