using Game.Scripts.Core;
using UnityEngine;

namespace Game.TestTools.GameFlow
{
    public class Test : MonoBehaviour
    {
        private void Update()
        {
            TestLevelCompleted();
            TestLevelFailed();
        }

        private void TestLevelCompleted()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameManager.Instance.OnLevelCompleted();
            }
        }

        private void TestLevelFailed()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.Instance.OnLevelFailed();
            }
        }
    }
}
