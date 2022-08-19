using DG.Tweening;
using Game.Scripts.Score;
using Game.Scripts.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class FailPanel : UIPanel
    {
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text bestScoreText;
        [SerializeField] private GameObject newHighScoreText;
        [SerializeField] private TMP_Text rankingPercentage;
        [SerializeField] private Image percentageSlider;
        
        public override void OnEnablePanel()
        {
            base.OnEnablePanel();
            
            currentScoreText.text = $"{ScoreHandler.Instance.currentScore.ToString()}";
            bestScoreText.text = $"PERSONAL BEST: {ScoreHandler.Instance.bestScore.ToString()}";

            var percentage = ScoreHandler.Instance.GetPercentage();
            PercentageRoundAnimation(percentage);
            percentageSlider.DOFillAmount(percentage, 1f);
            
            if (ScoreHandler.Instance.isNewHighScore) newHighScoreText.SetActive(true);
        }

        private void PercentageRoundAnimation(float percentage)
        {
            var percentageAsInt = (int)(percentage * 100);
            if (percentageAsInt > 98) percentageAsInt = 98;
            
            DOVirtual.Float(0, percentageAsInt, 1f, value =>
            {
                var currentPercentage = (int)value;
                rankingPercentage.text = $"%{currentPercentage.ToString()}";
            });
        }
    }
}
