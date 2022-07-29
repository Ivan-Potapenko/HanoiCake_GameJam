using UnityEngine;

namespace Gameplay {

    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData")]
    public class CharacterData : ScriptableObject {

        [SerializeField]
        private float _speed;
        public float Speed => _speed;

        [SerializeField]
        private float _accelerationSpeed;
        public float AccelerationSpeed => _accelerationSpeed;

        [SerializeField]
        private float _brakingSpeed;
        public float BrakingSpeed => _brakingSpeed;

        [SerializeField]
        private float _jumpForce;
        public float JumpForce => _jumpForce;

        [SerializeField]
        private float _climbJumpForce;
        public float ClimbJumpForce => _climbJumpForce;


        [SerializeField]
        private float _climbSpeed;
        public float ClimbSpeed => _climbSpeed;

        [SerializeField]
        private float _inAirBrakingSpeed;
        public float InAirBrakingSpeed => _inAirBrakingSpeed;

    }
}
