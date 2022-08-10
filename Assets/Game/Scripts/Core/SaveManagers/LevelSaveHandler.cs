using UnityEngine;

namespace Game.Scripts.Core.SaveManagers
{
    public class LevelSaveHandler : MonoBehaviour
    {
        private const string LevelKey = "levels";
        private const string LevelByOrderKey = "LevelByOrder";

        // Gets invoked if level is completed. Listener for "onLevelCompleted" event. 
        public void IncreaseLevel()
        { 
            PlayerPrefs.SetInt(LevelKey, GetLevelNumber() + 1);
        }

        public int GetLevelNumber()
        {
            return PlayerPrefs.GetInt(LevelKey, 0);
        }

        public void SetLevelByOrder(int levelIndex, int order)
        {
            PlayerPrefs.SetInt($"{LevelByOrderKey}{GetLevelNumber() + order}", levelIndex);
        }

        public int GetLevelIndex(int order)
        {
            return PlayerPrefs.GetInt(LevelByOrderKey + order, -1);
        }
    }
}
