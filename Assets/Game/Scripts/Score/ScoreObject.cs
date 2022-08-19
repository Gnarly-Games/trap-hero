using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Score
{
    public class ScoreObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private CanvasGroup canvasGroup;

        private Vector3 _originalScale;
        
        public void Initialize(int score)
        {
            scoreText.transform.localScale = Vector3.zero;
            scoreText.text = "+" + score;

            scoreText.transform.DOScale(1f, 0.4f);
            canvasGroup.DOFade(1f, 0.5f)
                .OnComplete(() =>
                {
                    canvasGroup.DOFade(0f, 0.5f)
                        .OnComplete(() => Destroy(gameObject));
                });
        }
    }
}
