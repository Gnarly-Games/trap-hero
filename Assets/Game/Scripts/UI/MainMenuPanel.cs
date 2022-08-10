using Game.Scripts.Core.SaveManagers;
using Game.Scripts.UI.Base;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class MainMenuPanel : UIPanel
    {
        [SerializeField] private TMP_Text levelCounter;

        // Gets invoked when level is created. Listener for "onLevelCreated" event. 
        public void SetLevelText(LevelSaveHandler saveManager)
        {
            levelCounter.text = $"LEVEL {saveManager.GetLevelNumber() + 1}";
        }
    }
}
