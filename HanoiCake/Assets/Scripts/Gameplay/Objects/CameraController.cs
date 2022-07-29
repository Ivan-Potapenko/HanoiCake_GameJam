using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

namespace Gameplay {

    [RequireComponent(typeof(EventListener))]
    public class CameraController : MonoBehaviour {

        [SerializeField]
        private CharacterInput _characterInput;

        [SerializeField]
        private float _characterSwitchTime;

        private float _distanceToCharacter;

        [SerializeField]
        private float _maxDistanceDiff;

        private bool _onChangeCoroutine;

        [SerializeField]
        private EventListener _updateEventListener;

        private void Start() {
            _distanceToCharacter = gameObject.transform.position.x - _characterInput.CurrentCharacterController.transform.position.x;
        }

        private void UpdateBehaviour() {
            Move();
        }

        private void Move() {
            if(_onChangeCoroutine) {
                return;
            }
            gameObject.transform.position = new Vector3(_distanceToCharacter + _characterInput.CurrentCharacterController.transform.position.x, 
                gameObject.transform.position.y, gameObject.transform.position.z);
        }

        private void OnEnable() {
            _characterInput.onCurrentCharacterChange += ChangeTrackedCharacter;
            _updateEventListener.ActionsToDo += UpdateBehaviour;
        }

        private void OnDisable() {
            _characterInput.onCurrentCharacterChange -= ChangeTrackedCharacter;
            _updateEventListener.ActionsToDo -= UpdateBehaviour;
        }

        private void ChangeTrackedCharacter() {
            if(_onChangeCoroutine) {
                StopCoroutine(ChangeTrackedCharacterCoroutine());
            }
            StartCoroutine(ChangeTrackedCharacterCoroutine());
        }

        private IEnumerator ChangeTrackedCharacterCoroutine() {
            _onChangeCoroutine = true;
            var distanceToCharacter =Mathf.Abs(gameObject.transform.position.x - _characterInput.CurrentCharacterController.transform.position.x);

            while (Mathf.Abs(_characterInput.CurrentCharacterController.transform.position.x + _distanceToCharacter - gameObject.transform.position.x) > _maxDistanceDiff) {
                var lerpStep = distanceToCharacter / (_characterSwitchTime / Time.deltaTime);
                gameObject.transform.position = new Vector3(
                    Mathf.Lerp(gameObject.transform.position.x, _characterInput.CurrentCharacterController.transform.position.x + _distanceToCharacter, lerpStep),
                    gameObject.transform.position.y, gameObject.transform.position.z);
                yield return null;
            }

            if(_characterInput.CurrentCharacterController.IsDead) {
                Time.timeScale = 0;
            }

            _onChangeCoroutine = false;
        }
    }

}
