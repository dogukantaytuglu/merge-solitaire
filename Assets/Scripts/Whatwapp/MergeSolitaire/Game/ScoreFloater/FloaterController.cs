using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.ScoreFloater
{
    public class FloaterController : MonoBehaviour
    {
        [SerializeField] private AnimationSettings _animationSettings;
        [SerializeField] private TextMeshPro _textMesh;

        private Sequence _sequence;

        public void Show(string message, Vector2 position)
        {
            _textMesh.text = message;
            transform.position = position;
            var targetPosition = position + new Vector2(0, _animationSettings.ScoreFloaterYMoveAmount);
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOPunchScale(Vector3.one * 0.8f, 0.7f));
            _sequence.Append(transform.DOMove(targetPosition, _animationSettings.ScoreFloaterMoveDuration));
            _sequence.Join(_textMesh.DOFade(0, _animationSettings.ScoreFloaterMoveDuration));
            _sequence.OnComplete(() => Destroy(gameObject));
        }

        private void OnDisable()
        {
            _sequence?.Kill(true);
        }
    }
}