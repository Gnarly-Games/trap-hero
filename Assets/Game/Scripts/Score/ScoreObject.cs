using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Score
{
    public class ScoreObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private CanvasGroup canvasGroup;
    
        public void Initialize(int score)
        {
            scoreText.text = "+" + score;
        
            canvasGroup.DOFade(1f, 0.25f)
                .OnComplete(() =>
                {
                    canvasGroup.DOFade(0f, 0.25f)
                        .OnComplete(() => Destroy(gameObject));
                });
        }
    }
}
