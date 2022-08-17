using System.Collections;
using Game.Scripts.Score;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public class Timer : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(IncreaseScore());
        }

        private IEnumerator IncreaseScore()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                ScoreHandler.Instance.IncreaseScore(1);
            }
        }
    }
}
