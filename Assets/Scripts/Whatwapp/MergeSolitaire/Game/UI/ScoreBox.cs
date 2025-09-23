using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Logger;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class ScoreBox : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private TMPro.TMP_Text _scoreText;

        [SerializeField] private AnimationSettings _animationSettings;
        private int _currentScore;

        private Tween _scoreAnimation;
        private Sequence _sequence;

        public void SetScore(int score, bool animate = true)
        {
            if ((score == 0) || (_currentScore >= score) || !animate)
            {
                SetImmediate(score);
                return;
            }

            UpdateScore(score);
        }

        private void SetImmediate(int score)
        {
            _scoreText.text = score.ToString();
            _currentScore = score;
        }

        private void UpdateScore(int score)
        {
            var currentScore = _currentScore;
            _currentScore = score;
            var delta = score - currentScore;
            var step = Mathf.Max(1, delta / 10);
            var stepCount = delta / step;
            _sequence?.Kill(true);
            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() =>
            {
                currentScore += step;
                currentScore = Mathf.Min(currentScore, score);
                _scoreText.text = currentScore.ToString();
                PlayPunchAnimation();
            });
            _sequence.AppendInterval(_animationSettings.ScoreAnimationDuration);

            _sequence.SetLoops(stepCount);
        }

        private void PlayPunchAnimation()
        {
            _scoreAnimation?.Kill(true);
            _scoreAnimation = _scoreText.transform.DOPunchScale(Vector3.one * _animationSettings.ScoreAnimationPower,
                _animationSettings.ScoreAnimationDuration);
        }
    }
}