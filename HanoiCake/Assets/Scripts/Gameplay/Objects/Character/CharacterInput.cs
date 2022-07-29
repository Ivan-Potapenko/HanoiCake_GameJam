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

        private bool _freez = false;

        [SerializeField]
        private EventListener _updateEventListener;

        [SerializeField]
        private EventListener _fixedUpdateEventListener;

        private void OnEnable() {
            _updateEventListener.ActionsToDo += UpdateBehaviour;
            _fixedUpdateEventListener.ActionsToDo += FixedUpdateBehaviour;
            _bottomCharacterController.onDead += FreezOnBottomCharacter;
            _topCharacterController.onDead += FreezOnTopCharacter;
        }

        private void OnDisable() {
            _updateEventListener.ActionsToDo -= UpdateBehaviour;
            _fixedUpdateEventListener.ActionsToDo -= FixedUpdateBehaviour;
            _topCharacterController.onDead -= FreezOnTopCharacter;
            _bottomCharacterController.onDead -= FreezOnBottomCharacter;
        }

        private void FreezOnBottomCharacter() {
            _bottomCharacterSelected = true;
            _freez = true;
        }

        private void FreezOnTopCharacter() {
            _bottomCharacterSelected = false;
            _freez = true;
        }

        private void UpdateBehaviour() {
            JumpInput();
            ChangeCharacter();
        }

        private void FixedUpdateBehaviour() {
            MoveInput();
        }

        private void ChangeCharacter() {
            if (Input.GetKeyDown(KeyCode.Q) && !_freez) {
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
