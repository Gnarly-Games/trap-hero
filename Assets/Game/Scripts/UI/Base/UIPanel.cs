using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.UI.Base
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        private const float UIFadeOutTime = 0.25f;
        
        private CanvasGroup _canvasGroup;

        public virtual void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void OnEnablePanel()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _canvasGroup.DOFade(1f, UIFadeOutTime);
        }

        public virtual void OnDisablePanel(Action onComplete = null)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _canvasGroup.DOFade(0f, UIFadeOutTime)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}
