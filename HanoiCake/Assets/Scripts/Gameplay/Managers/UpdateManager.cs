using UnityEngine;
using Events;

namespace Gameplay {

    [RequireComponent(typeof(EventDispatcher))]
    public class UpdateManager : MonoBehaviour {

        [SerializeField]
        private EventDispatcher _updateEventDispatcher;

        [SerializeField]
        private EventDispatcher _fixedUpdateEventDispatcher;

        void Update() {
            _updateEventDispatcher.Dispatch();
        }

        private void FixedUpdate() {
            _fixedUpdateEventDispatcher.Dispatch();
        }
    }
}
