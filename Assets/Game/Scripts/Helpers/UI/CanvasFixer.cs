using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Helpers.UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasFixer : MonoBehaviour
    {
        CanvasScaler cs;

        private void Start()
        {
            cs = GetComponent<CanvasScaler>();

            float screenRatio;

            float rat = (float)Screen.width / (float)Screen.height;

            if (rat < .56f)
            {
                screenRatio = 0f;
                Camera.main.fieldOfView += 8;
            }

            else if (rat >= .56f && rat < .624f)
                screenRatio = .5f;
            else
                screenRatio = 1f;

            cs.matchWidthOrHeight = screenRatio;
        }
    }
}
