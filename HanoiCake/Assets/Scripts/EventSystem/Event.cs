using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events {

    [CreateAssetMenu(fileName = "newEvent", menuName = "Event")]
    public class Event : ScriptableObject {

        private List<Action> _listeners;

        public void AddListener(Action action) {
            if (_listeners == null) {
                _listeners = new List<Action>();
            }
            if (!_listeners.Contains(action)) {
                _listeners.Add(action);
            }
        }

        public void RemoveListener(Action action) {
            if (_listeners == null) {
                return;
            }
            if (_listeners.Contains(action)) {
                _listeners.Remove(action);
            }
        }

        public void Dispatch() {
            if (_listeners == null) {
                return;
            }
            for (int i = _listeners.Count - 1; i >= 0; i--) {
                _listeners[i]();
            }
        }

    }
}