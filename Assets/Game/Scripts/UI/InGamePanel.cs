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

        [SerializeField] private Color highlightColor;
        [SerializeField] private Color originalColor;
        
        [SerializeField] private float scoreUpdateInterval;
        [SerializeField] private int milestone;
            
        private int _currentScore;
        private int _targetScore;

        public override void Start()
        {
            base.Start();
            StartCoroutine(UpdateScoreText());
        }

        public void UpdateScoreValue(int score)
        {
            _targetScore = score;
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
