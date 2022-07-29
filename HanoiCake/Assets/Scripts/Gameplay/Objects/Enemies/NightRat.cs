using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {

    public class NightRat : Enemy {

        [SerializeField]
        private float _speed;

        [SerializeField]
        private CharacterController _topCharacterController;

        [SerializeField]
        private float distanceToCharacter;

        private bool _isMoving;

        private void OnEnable() {
            _topCharacterController.changeInLight += CheckInLightState;
        }

        private void OnDisable() {
            _topCharacterController.changeInLight -= CheckInLightState;
        }

        private void CheckInLightState() {
            if (!_topCharacterController.InLight) {
                StartToMove();
            } else {
                Stop();
            }
        }

        private void StartToMove() {
            StartCoroutine(MoveToCharacter());
        }

        private void Stop() {
            ResetPosition();
            if (_isMoving) {
                StopCoroutine(MoveToCharacter());
            }
        }

        private void ResetMove() {
            ResetPosition();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.TryGetComponent<CandleLight>(out var candleLight)) {
                ResetMove();
            }
        }

        private IEnumerator MoveToCharacter() {
            _isMoving = true;

            ResetPosition();
            while (!_topCharacterController.InLight) {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _topCharacterController.transform.position, _speed * Time.deltaTime);
                yield return null;
            }

            _isMoving = false;
        }

        private void ResetPosition() {
            gameObject.transform.position = new Vector3(_topCharacterController.transform.position.x, _topCharacterController.transform.position.y + distanceToCharacter,
                gameObject.transform.position.z);
        }

    }

}
