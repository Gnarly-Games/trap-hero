using DG.Tweening;
using Game.Scripts.UI.Base;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class InGamePanel : UIPanel
    {
        [Separator("Hit Effect")]
        [SerializeField] private Image hitEffect;
        [SerializeField] private float hitEffectDuration;

        [Separator("Gold Panel")] 
        [SerializeField] private TMP_Text goldPanel;
        
        public void OnPlayerReceivedDamage()
        {
            hitEffect.DOFade(0.3f, hitEffectDuration)
                .OnComplete(() =>
                {
                    hitEffect.DOFade(0f, hitEffectDuration);
                });
        }

        public void OnGoldCollected(int gold)
        {
            goldPanel.text = gold.ToString();
        }
    }
}
