using System.Collections;
using DG.Tweening;
using Game.Scripts.UI.Base;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class InGamePanel : UIPanel
    {
        public TMP_Text scoreText;
        public CanvasGroup inGameDebug;

        [SerializeField] private Color highlightColor;
        [SerializeField] private Color originalColor;
        
        [SerializeField] private float scoreUpdateInterval;
        [SerializeField] private int milestone;
            
        private int _currentScore;
        private int _targetScore;
        private int _clickCount;

        public override void Start()
        {
            base.Start();
            StartCoroutine(UpdateScoreText());
        }

        public void UpdateScoreValue(int score)
        {
            _targetScore = score;
        }

        public void IncreaseClickCount()
        {
            _clickCount++;
            if (_clickCount == 10)
            {
                inGameDebug.alpha = 1f;
            }
        }

        private IEnumerator UpdateScoreText()
        {
            while (true)
            {
                yield return new WaitForSeconds(scoreUpdateInterval);
                if (_targetScore == _currentScore) continue;
                
                _currentScore++;
                scoreText.text = _currentScore.ToString();

                if (_currentScore % milestone != 0) continue;
                scoreText.transform.DOScale(1.2f, 0.25f)
                    .OnComplete(() => scoreText.transform.DOScale(1f, 0.25f));
                scoreText.DOColor(highlightColor, 0.25f)
                    .OnComplete(() => scoreText.DOColor(originalColor, 0.25f));
            }
        }
    }
}
