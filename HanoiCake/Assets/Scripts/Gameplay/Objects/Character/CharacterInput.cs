using Events;
using System;
using UnityEngine;

namespace Gameplay {


    [RequireComponent(typeof(EventListener))]
    public class CharacterInput : MonoBehaviour {

        [SerializeField]
        private CharacterController _topCharacterController;

        [SerializeField]
        private CharacterController _bottomCharacterController;

        public CharacterController CurrentCharacterController => _bottomCharacterSelected ? _bottomCharacterController : _topCharacterController;
        public CharacterController InactiveCharacterController => !_bottomCharacterSelected ? _bottomCharacterController : _topCharacterController;

        public Action onCurrentCharacterChange = delegate { };

        private bool _bottomCharacterSelected = true;

        [SerializeField]
        private EventListener _updateEventListener;

        [SerializeField]
        private EventListener _fixedUpdateEventListener;

        private void OnEnable() {
            _updateEventListener.ActionsToDo += UpdateBehaviour;
            _fixedUpdateEventListener.ActionsToDo += FixedUpdateBehaviour;
        }

        private void OnDisable() {
            _updateEventListener.ActionsToDo -= UpdateBehaviour;
            _fixedUpdateEventListener.ActionsToDo -= FixedUpdateBehaviour;
        }


        private void UpdateBehaviour() {
            JumpInput();
            ChangeCharacter();
        }

        private void FixedUpdateBehaviour() {
            MoveInput();
        }

        private void ChangeCharacter() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                _bottomCharacterSelected = !_bottomCharacterSelected;
                onCurrentCharacterChange.Invoke();
            }
        }


        private void JumpInput() {
            if (Input.GetKeyDown(KeyCode.W)) {
                CurrentCharacterController.Jump();
            }
        }

        private void MoveInput() {
            if (Input.GetKey(KeyCode.A)) {
                CurrentCharacterController.Move(CharacterController.Direction.Left);
            } else if (Input.GetKey(KeyCode.D)) {
                CurrentCharacterController.Move(CharacterController.Direction.Right);
            } else {
                CurrentCharacterController.Stand();
            }
            InactiveCharacterController.Stand();
        }
    }
}
