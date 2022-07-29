using System;
using UnityEngine;

namespace Events {
    public class EventListener : MonoBehaviour {

        [SerializeField]
        private Event _someEvent;

        public event Action ActionsToDo = delegate { };

        private void OnEnable() {
            _someEvent.AddListener(OnEventHappend);
        }

        private void OnDisable() {
            _someEvent.RemoveListener(OnEventHappend);
        }

        private void OnEventHappend() {
            ActionsToDo.Invoke();
        }

    }
}