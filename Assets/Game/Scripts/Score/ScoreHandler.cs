using DG.Tweening;
using MyBox;
using UnityEngine;

namespace Game.Scripts.Score
{
    public class ScoreHandler : Singleton<ScoreHandler>
    {
        [SerializeField] private ScoreObject scoreObject;

        public void ShowScore(Vector3 spawnPosition, int score)
        {
            var spawnedObject = Instantiate(scoreObject);
            spawnedObject.Initialize(score);
            spawnedObject.transform.position = spawnPosition;
        }
    }
}
