using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Player
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Slider healthBar;

        private void Start()
        {
            healthBar.value = playerController.health / 100f;
        }

        public void OnDamageReceived()
        {
            healthBar.value = playerController.health / 100f;
        }
    }
}
