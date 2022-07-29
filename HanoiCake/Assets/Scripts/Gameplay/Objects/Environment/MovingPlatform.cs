using Events;
using System.Collections;
using UnityEngine;

namespace Gameplay {

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EventListener))]
    public class MovingPlatform : Environment {

        [SerializeField]
        private Transform _leftPoint;

        [SerializeField]
        private Transform _rightPoint;

        [SerializeField]
        private float _xSpeed;

        [SerializeField]
        private float _ySpeed;

        [SerializeField]
        private EventListener _fixedUpdateEventListener;

        [SerializeField]
        private float _brakingSpeed;

        private Rigidbody2D _rigidbody;

        [SerializeField]
        private bool _moveLeft = false;

        [SerializeField]
        private float _minSpeed;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() {
            _fixedUpdateEventListener.ActionsToDo += FixedUpdateBehaviour;
        }

        private void OnDisable() {
            _fixedUpdateEventListener.ActionsToDo -= FixedUpdateBehaviour;
        }

        private void FixedUpdateBehaviour() {
            Move();
        }

        private void Move() {
            if (_isBraking) {
                return;
            }
            _rigidbody.velocity = new Vector2(_xSpeed * (_moveLeft ? -1 : 1), _ySpeed);
            if ((_moveLeft && gameObject.transform.position.x <= _leftPoint.transform.position.x) ||
                (!_moveLeft && gameObject.transform.position.x >= _rightPoint.transform.position.x)) {
                _moveLeft = !_moveLeft;
                StartCoroutine(Brake());
            }
        }

        private IEnumerator Brake() {
            _isBraking = true;
            while (Mathf.Abs(_rigidbody.velocity.x) > _minSpeed || Mathf.Abs(_rigidbody.velocity.y) > _minSpeed) {
                _rigidbody.velocity = new Vector2(GetLerpSpeed(_rigidbody.velocity.x), GetLerpSpeed(_rigidbody.velocity.y));
                yield return new WaitForFixedUpdate();
            }
            _isBraking = false;
        }

        private float GetLerpSpeed(float speed) {
            return Mathf.Lerp(speed, 0, _brakingSpeed);
        }

        public override Vector2 GetEnvironmentVelocity() {
            return _rigidbody.velocity;
        }

    }
}

