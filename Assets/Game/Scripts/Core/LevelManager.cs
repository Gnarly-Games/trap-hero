using System.Linq;
using Game.Scripts.Core.SaveManagers;
using Game.Scripts.UI;
using MyBox;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class LevelManager : MonoBehaviour
    {
        [Separator("Levels")]
        [SerializeField] private Level testLevel;
        [SerializeField] private Transform levelParent;
        [SerializeField] private Level[] levelPool;

        [Separator("Saving")]
        [SerializeField] private LevelSaveHandler saveManager;

        private void Start()
        {
            CreateLevel();
        }

        private void CreateLevel()
        {
            if (testLevel == null && levelPool.Length == 0) return;

            var levelNumber = GetLevelNumber();
            var levelToCreate = GetLevel(levelNumber);
            
            Instantiate(levelToCreate, levelParent);
            GameManager.Instance.onLevelCreated?.Invoke(saveManager);
        }

        private int GetLevelNumber()
        {
            if (levelPool.Length == 0) return 0;

            var currentLevel = saveManager.GetLevelNumber();
            var levelIndex = saveManager.GetLevelIndex(currentLevel);

            if (levelIndex != -1) return levelIndex; // -1 is the escape character. If -1 returns it means that levels that saved in order is empty.
            if (levelPool.Length > currentLevel) return currentLevel;

            return GetRandomLevel(currentLevel);
        }

        private int GetRandomLevel(int currentLevel)
        {
            // Add all levels that marked as "random level" into new list.
            var randomLevelPool = levelPool.Where(pickedLevel => pickedLevel.isRandomLevel).ToList();
            var orderCounter = 0;

            // Pick random level and place it into save data as next level.
            while (randomLevelPool.Count > 0)
            {
                var randomSelectedLevel = randomLevelPool.GetRandom();
                var selectedLevelIndex = levelPool.IndexOfItem(randomSelectedLevel);

                saveManager.SetLevelByOrder(selectedLevelIndex, orderCounter);

                randomLevelPool.Remove(randomSelectedLevel);
                orderCounter++;
            }

            return saveManager.GetLevelIndex(currentLevel);
        }

        private Level GetLevel(int levelID)
        {
            if (testLevel != null) return testLevel;

            return levelPool[levelID];
        }
    }
}
