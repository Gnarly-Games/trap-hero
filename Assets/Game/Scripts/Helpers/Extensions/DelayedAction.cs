using System;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.Helpers.Extensions
{
    public class DelayedAction
    {
        private Action _action;
        private float _delay;

        public DelayedAction(Action action,float delay)
        {
            _action = action;
            _delay = delay;
        }

        public void Execute(MonoBehaviour parent)
        {
            parent.StartCoroutine(GetCoroutine());
        }

        private IEnumerator GetCoroutine()
        {
            yield return new WaitForSeconds(_delay);
            _action?.Invoke();
        }
    }
}

