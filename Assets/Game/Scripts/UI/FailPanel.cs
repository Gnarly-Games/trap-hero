using Game.Scripts.UI.Base;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class FailPanel : UIPanel
    {
        [SerializeField] private TMP_Text killCount;

        public override void OnEnablePanel()
        {
            base.OnEnablePanel();
            killCount.text = $"Kill Count: {SpawnMonitor.Instance.Dead}";
        }
    }
}
