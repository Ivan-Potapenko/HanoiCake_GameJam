using Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {

    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController : MonoBehaviour {

        [SerializeField]
        private CharacterData _characterData;

        private Rigidbody2D _rigidbody;

        public enum CharacterState {
            MoveOnGround,
            Ñlimb,
            StayOnGround,
            MoveInAir,
            StayInAir,
        }

        public enum Direction {
            Left,
            Right
        }


        public Action onStateChange = delegate { };
        private CharacterState _currentState = CharacterState.StayInAir;
        public CharacterState CurrentState {
            get {
                return _currentState;
            }
            set {
                if (value != _currentState) {
                    _currentState = value;
                    onStateChange?.Invoke();
                }
            }
        }

        public Action onDirectionChange = delegate { };
        private Direction _currentDirection = Direction.Right;
        public Direction CurrentDirection {
            get {
                return _currentDirection;
            }
            set {
                if (value != _currentDirection) {
                    _currentDirection = value;
                    onDirectionChange?.Invoke();
                }
            }
        }

        private bool _isGrounded;

        [SerializeField]
        private EventDispatcher _deadEventDispatcher;

        [SerializeField]
        private bool _isDead;

        public Action onDead = delegate { };

        public Action onCurrentConnectEnviromentChange = delegate { };
        private Environment _currentConnectEnvironment = null;
        private Environment CurrentConnectEnvironment {
            get { return _currentConnectEnvironment; }
            set {
                if (_currentConnectEnvironment != value) {
                    _currentConnectEnvironment = value;
                    onCurrentConnectEnviromentChange?.Invoke();
                }
            }
        }

        private void Start() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }



        public void Move(Direction direction) {
            if (CurrentState == CharacterState.Ñlimb && !_isGrounded && _currentDirection == direction) {
                return;
            }

            if (_isGrounded) {
                CurrentState = CharacterState.MoveOnGround;
            } else {
                CurrentState = CharacterState.MoveInAir;
            }
            CurrentDirection = direction;

            SetMoveVelocity(Mathf.Lerp(_rigidbody.velocity.x - GetCurrentEnvironmentVelocity().x,
                _characterData.Speed * ConvertDirectionToVector(_currentDirection).x, _characterData.AccelerationSpeed));
        }

        public void Stand() {
            if (CurrentState == CharacterState.Ñlimb && !_isGrounded) {
                return;
            }
            if (_isGrounded) {
                CurrentState = CharacterState.StayOnGround;
            } else {
                CurrentState = CharacterState.StayInAir;
            }
            SetMoveVelocity(Mathf.Lerp(_rigidbody.velocity.x - GetCurrentEnvironmentVelocity().x,
                0, _isGrounded ? _characterData.BrakingSpeed : _characterData.InAirBrakingSpeed));
        }

        private void SetMoveVelocity(float xSpeed) {
            Debug.LogWarning(xSpeed);
            SetMoveVelocity(xSpeed, _rigidbody.velocity.y);
        }

        private void SetMoveVelocity(float xSpeed, float ySpeed) {
            _rigidbody.velocity = new Vector2(xSpeed, ySpeed) + GetCurrentEnvironmentVelocity();
        }

        private Vector2 GetCurrentEnvironmentVelocity() {
            return CurrentConnectEnvironment != null && !CurrentConnectEnvironment.IsBraking ? _currentConnectEnvironment.GetEnvironmentVelocity() : Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            CheckEnvironmentCollision(collision);
        }

        private void OnCollisionStay2D(Collision2D collision) {
            CheckEnvironmentCollision(collision, false);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            CheckEnvironmentExit(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            CheckEnemyEnter(collision);
        }

        private void CheckEnemyEnter(Collider2D collision) {
            if (!collision.gameObject.TryGetComponent<Enemy>(out var enemy)) {
                return;
            }
            Die();
        }

        private void Die() {
            _deadEventDispatcher.Dispatch();
            _isDead = true;
            onDead.Invoke();
        }

        private void CheckEnvironmentExit(Collision2D collision) {
            if (!collision.gameObject.TryGetComponent<Environment>(out var environment) || environment != CurrentConnectEnvironment) {
                return;
            }
            if (environment.isGround) {
                _isGrounded = false;
            }
            _currentConnectEnvironment = null;
        }

        private void CheckEnvironmentCollision(Collision2D collision, bool checkGround = true) {
            if (!collision.gameObject.TryGetComponent<Environment>(out var environment)) {
                return;
            }
            foreach (var contactPoint in collision.contacts) {
                if (checkGround && contactPoint.normal == Vector2.up) {
                    CurrentConnectEnvironment = environment;
                    environment.isGround = true;
                    _isGrounded = true;
                    return;
                }
                if (TryToStartClimbing(environment, contactPoint.normal)) {
                    return;
                }
            }
        }

        private bool TryToStartClimbing(Environment environment, Vector2 pointNormal) {
            if (_isGrounded || _rigidbody.velocity.y > 0 || CurrentState == CharacterState.Ñlimb) {
                return false;
            }
            if (pointNormal == ConvertDirectionToVector(GetReverseDirection(CurrentDirection))) {
                environment.isGround = false;
                CurrentConnectEnvironment = environment;
                StartCoroutine(StartClimbingCoroutine());
            }
            return true;
        }

        private IEnumerator StartClimbingCoroutine() {
            CurrentState = CharacterState.Ñlimb;
            while (!_isGrounded && CurrentState == CharacterState.Ñlimb && _currentConnectEnvironment != null) {
                SetMoveVelocity(0, _characterData.ClimbSpeed);
                yield return new WaitForFixedUpdate();
            }
        }

        private Direction GetReverseDirection(Direction direction) {
            switch (direction) {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                default:
                    return Direction.Right;
            }
        }

        private static Vector2 ConvertDirectionToVector(Direction direction) {
            switch (direction) {
                case Direction.Left:
                    return Vector2.left;
                case Direction.Right:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
        }

        public void Jump() {
            if (CurrentState == CharacterState.Ñlimb) {
                var reverseDirection = GetReverseDirection(_currentDirection);
                Jump(Vector2.up + ConvertDirectionToVector(reverseDirection) * 2f, _characterData.ClimbJumpForce);
                CurrentState = CharacterState.StayInAir;
                CurrentDirection = reverseDirection;
            } else if (_isGrounded) {
                Jump(Vector2.up, _characterData.JumpForce);
            }
        }

        public void Jump(Vector2 jumpVector, float jumpForce) {
            _rigidbody.velocity = jumpVector * jumpForce + GetCurrentEnvironmentVelocity();
        }

    }
}

