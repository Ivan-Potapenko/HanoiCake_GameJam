using Events;
using System.Collections;
using UnityEngine;

namespace Gameplay {

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EventListener))]
    public class RatWatchman : Enemy {

        [SerializeField]
        private Transform _leftPoint;

        [SerializeField]
        private Transform _rightPoint;

        [SerializeField]
        private float _xSpeed;

        [SerializeField]
        private EventListener _fixedUpdateEventListener;

        private Rigidbody2D _rigidbody;

        [SerializeField]
        private bool _moveLeft = false;

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
            _rigidbody.velocity = new Vector2(_xSpeed * (_moveLeft ? -1 : 1), 0);
            if ((_moveLeft && gameObject.transform.position.x <= _leftPoint.transform.position.x) ||
                (!_moveLeft && gameObject.transform.position.x >= _rightPoint.transform.position.x)) {
                _moveLeft = !_moveLeft;
            }
        }

    }
}
