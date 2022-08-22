using Game.Scripts.Core.SaveManagers;
using GameAnalyticsSDK;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core
{
    public class GameManager : Singleton<GameManager>
    {
        private const string SceneName = "Game";
        private const string LevelKey = "Level";
        
        public UnityEvent onGameInitialized;
        public UnityEvent<LevelSaveHandler> onLevelCreated;
        public UnityEvent onLevelStarted;
        public UnityEvent onLevelCompleted;
        public UnityEvent onLevelFailed;
        public UnityEvent onLevelEnded;

        public bool isGameRunning;

        private void Awake() 
        {
            Application.targetFrameRate = 60;
            GameAnalytics.Initialize();
        }
        
        private void Start()
        {
            onGameInitialized?.Invoke();
        }

        public void OnStartGame()
        {
            var currentLevel = PlayerPrefs.GetInt(LevelKey, 1);
            GameAnalytics.NewDesignEvent($"level_{currentLevel}_start");
            
            onLevelStarted?.Invoke();
            isGameRunning = true;
        }

        public void OnLevelCompleted()
        {
            onLevelCompleted?.Invoke();
            onLevelEnded?.Invoke();
        }

        public void OnLevelFailed()
        {
            var currentLevel = PlayerPrefs.GetInt(LevelKey, 1);
            GameAnalytics.NewDesignEvent($"level_{currentLevel}_end");

            currentLevel++;
            PlayerPrefs.SetInt(LevelKey, currentLevel);
            
            onLevelFailed?.Invoke();
            onLevelEnded?.Invoke();
        }

        public static void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
    }
}
