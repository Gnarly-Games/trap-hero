using Game.Scripts.UI.Base;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIPanel mainMenu;
        [SerializeField] private UIPanel inGame;
        [SerializeField] private UIPanel successScreen;
        [SerializeField] private UIPanel failScreen;

        private UIPanel _activePanel;

        // Gets invoked when game initialized. Listener for "onGameInitialized" event.
        public void Initialize()
        {
            _activePanel = mainMenu;
        }

        // Gets invoked when level is started. Listener for "onLevelStarted" event.
        public void ShowInGamePanel()
        {
            ActivatePanel(inGame);
        }

        // Gets invoked if level is completed. Listener for "onLevelCompleted" event.
        public void ShowSuccessScreen()
        {
            ActivatePanel(successScreen);
        }
        
        // Gets invoked if level is failed. Listener for "onLevelFailed" event.
        public void ShowFailScreen()
        {
            ActivatePanel(failScreen);
        }

        private void ActivatePanel(UIPanel panelToEnable)
        {
            if (_activePanel != null)
            {
                _activePanel.OnDisablePanel(panelToEnable.OnEnablePanel);
            }
            else
            {
                panelToEnable.OnEnablePanel();
            }
            
            _activePanel = panelToEnable;
        }
    }
}
