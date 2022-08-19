using System.Collections;
using Game.Scripts.Core;
using Game.Scripts.Score;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public class Timer : MonoBehaviour
    {
        private void Start()
        {
        }

        private IEnumerator IncreaseScore()
        {
            while (GameManager.Instance.isGameRunning)
            {
                yield return new WaitForSeconds(1f);
                ScoreHandler.Instance.IncreaseScore(1);
            }
        }
    }
}
