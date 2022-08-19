using Game.Scripts.Core.SaveManagers;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core
{
    public class GameManager : Singleton<GameManager>
    {
        private const string SceneName = "Game";
        
        public UnityEvent onGameInitialized;
        public UnityEvent<LevelSaveHandler> onLevelCreated;
        public UnityEvent onLevelStarted;
        public UnityEvent onLevelCompleted;
        public UnityEvent onLevelFailed;
        public UnityEvent onLevelEnded;

        public bool isGameRunning;

        private void Awake() {
            Application.targetFrameRate = 60;
        }
        
        private void Start()
        {
            onGameInitialized?.Invoke();
        }

        public void OnStartGame()
        {
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
            onLevelFailed?.Invoke();
            onLevelEnded?.Invoke();
        }

        public static void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
    }
}
