using Game.Scripts.Enemy;
using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        other.GetComponent<EnemyController>().GetDamage();
    }
}
