using System;
using Game.Scripts.UI;
using MyBox;
using UnityEngine;

namespace Game.Scripts.Score
{
    public class ScoreHandler : Singleton<ScoreHandler>
    {
        public int currentScore;
        public int bestScore;
        public bool isNewHighScore;

        [SerializeField] private int highestScoreOnRanking;
        [SerializeField] private AnimationCurve rankingCurve;
        [SerializeField] private ScoreObject scoreObject;
        [SerializeField] private InGamePanel inGamePanel;

        private const string SerializeKey = "Score";
        
        private AnimationCurve _inverseRankingCurve;

        private void Start()
        {
            CreateInverseRankingCurve();
        }

        public void ShowScore(Vector3 spawnPosition, int score)
        {
            var spawnedObject = Instantiate(scoreObject);
            spawnedObject.Initialize(score);
            spawnedObject.transform.position = spawnPosition;
        }
        
        public void IncreaseScore(int score)
        {
            currentScore += score;
            inGamePanel.UpdateScoreValue(currentScore);
        }

        public void Save()
        {
            Load();

            if (currentScore <= bestScore) return;
            
            bestScore = currentScore;
            PlayerPrefs.SetInt(SerializeKey, bestScore);
            isNewHighScore = true;
        }

        private void Load()
        {
            bestScore = PlayerPrefs.GetInt(SerializeKey, 0);
        }

        public float GetPercentage()
        {
            if (currentScore >= highestScoreOnRanking) return 0.98f;
            
            var normalizedValue = (float) currentScore / highestScoreOnRanking;
            var percentage = _inverseRankingCurve.Evaluate(normalizedValue);
            

            return percentage;
        }

        private void CreateInverseRankingCurve()
        {
            _inverseRankingCurve = new AnimationCurve();
            for (var i = 0; i < rankingCurve.length; i++)
            {
                var inverseKey = new Keyframe(rankingCurve.keys[i].value, rankingCurve.keys[i].time);
                _inverseRankingCurve.AddKey(inverseKey);
            }
        }
    }
}
