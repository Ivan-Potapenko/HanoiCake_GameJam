using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using System;

namespace Gameplay
{
    public class Character_Animator : MonoBehaviour
    {
        [SerializeField]
        private Animator _animatorLeg1;

        [SerializeField]
        private Animator _animatorLeg2;

        private string _direction = "Right";

        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _characterController.onStateChange += UpdateAnimation;
            _characterController.onDirectionChange += UpdateDirestion;
        }

        private void OnDisable()
        {
            _characterController.onStateChange -= UpdateAnimation;
            _characterController.onDirectionChange -= UpdateDirestion;
        }

        private void UpdateDirestion()
        {
            if(_characterController.CurrentDirection.ToString()!= _direction)
            {
                _characterController.transform.Rotate(0, 180, 0);
                _direction = _characterController.CurrentDirection.ToString();
            }
        }

        private void UpdateAnimation()
        {
            switch (_characterController.CurrentState)
            {
                case CharacterController.CharacterState.StayOnGround:
                    {
                        EndWalkingAnimation();
                        EndJumpingAnimation();
                        break;
                    }
                case CharacterController.CharacterState.MoveOnGround:
                    {
                        StartWalkingAnimation();
                        EndJumpingAnimation();
                        break;
                    }
                case CharacterController.CharacterState.StayInAir:
                    {
                        EndWalkingAnimation();
                        StartJumpingAnimation();
                        break;
                    }
                case CharacterController.CharacterState.MoveInAir:
                    {
                        EndWalkingAnimation();
                        StartJumpingAnimation();
                        break;
                    }
                case CharacterController.CharacterState.Ñlimb:
                    {
                        EndWalkingAnimation();
                        StartJumpingAnimation();
                        break;
                    }
            }
        }

        private void StartWalkingAnimation()
        {
            _animatorLeg1.SetBool("leg_is_go",true);
            _animatorLeg2.SetBool("leg2_is_go", true);
        }

        private void EndWalkingAnimation()
        {
            _animatorLeg1.SetBool("leg_is_go", false);
            _animatorLeg2.SetBool("leg2_is_go", false);
        }
        
        private void StartJumpingAnimation()
        {
            _animatorLeg1.SetBool("leg_is_jump", true);
            _animatorLeg2.SetBool("leg2_is_jump", true);
        }

        private void EndJumpingAnimation()
        {
            _animatorLeg1.SetBool("leg_is_jump", false);
            _animatorLeg2.SetBool("leg2_is_jump", false);
        }
    }
}