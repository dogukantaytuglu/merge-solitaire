using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.Cheats
{
    public class CheatManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        [SerializeField] private float slowMoTimeScale = 0.2f;
        private bool _isSlowMoActive;
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartSlowMo();
            }

            else
            {
                RestoreSlowMo();
            }
        }

        private void StartSlowMo()
        {
            if (_isSlowMoActive) return;
            _isSlowMoActive = true;
            Time.timeScale = slowMoTimeScale;
        }

        private void RestoreSlowMo()
        {
            if (_isSlowMoActive == false) return;
            _isSlowMoActive = false;
            Time.timeScale = 1f;
        }
    }
}