using Game.Scripts.Core.SaveManagers;
using GameAnalyticsSDK;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.UI
{
    public class GameManager : Singleton<GameManager>
    {
        private const string SceneName = "Game";
        
        public UnityEvent onGameInitialized;
        public UnityEvent<LevelSaveHandler> onLevelCreated;
        public UnityEvent onLevelStarted;
        public UnityEvent onLevelCompleted;
        public UnityEvent onLevelEnded;

        public bool isGameRunning;

        private int _matchCount;

        private const string MatchCountKey = "MatchID";

        private void Awake() 
        {
            Application.targetFrameRate = 60;
        }
        
        private void Start()
        {
            onGameInitialized?.Invoke();
            _matchCount = PlayerPrefs.GetInt(MatchCountKey, 0);
        }

        public void OnStartGame()
        {
            _matchCount++;
            PlayerPrefs.SetInt(MatchCountKey, _matchCount);
            
            GameAnalytics.NewDesignEvent($"match_start_{_matchCount.ToString()}");

            onLevelStarted?.Invoke();
            isGameRunning = true;
        }

        public void OnLevelCompleted()
        {
            GameAnalytics.NewDesignEvent($"match_end_{_matchCount.ToString()}");
            
            onLevelCompleted?.Invoke();
            onLevelEnded?.Invoke();
        }

        public static void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
    }
}
