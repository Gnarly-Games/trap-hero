using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Helpers.Cooldown
{
    public class CooldownHandler : MonoBehaviour
    {
        private readonly List<CooldownData> _coolDowns = new List<CooldownData>();
        
        private void Update()
        {
            HandleCooldownList();
        }

        private void HandleCooldownList()
        {
            var deltaTime = Time.deltaTime;
            
            for (var i = _coolDowns.Count - 1; i >= 0; i--)
            {
                if (!_coolDowns[i].DecrementCooldown(deltaTime)) continue;
                
                var currentCooldownData = _coolDowns[i];
                currentCooldownData.ActionToExecute?.Invoke();

                _coolDowns.RemoveAt(i);
            }
        }

        public void PutSkillOnCooldown(IHasCooldown skill)
        {
            var coolDownData = new CooldownData(skill);
            _coolDowns.Add(coolDownData);
        }
    }

    public class CooldownData
    {
        public Action ActionToExecute { get;}
        private float RemainingCooldownDuration { get; set; }

        public CooldownData(IHasCooldown skill)
        {
            RemainingCooldownDuration = skill.CooldownDuration;
            ActionToExecute = skill.ActionToExecute;
        }

        public bool DecrementCooldown(float deltaTime)
        {
            RemainingCooldownDuration = Mathf.Max(RemainingCooldownDuration - deltaTime, 0f);

            return RemainingCooldownDuration == 0f;
        }
    }
}
