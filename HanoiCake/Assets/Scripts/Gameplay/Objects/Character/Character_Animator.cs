using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

namespace Gameplay
{
    public class Character_Animator : MonoBehaviour
    {
        [SerializeField]
        private Animator _animatorLeg1;

        [SerializeField]
        private Animator _animatorLeg2;

        [SerializeField]
        private CharacterController _CharacterController;

        [SerializeField]
        private EventListener _updateEventListener;


        private void OnEnable()
        {
            _updateEventListener.ActionsToDo += UpdateBehaviour;
        }

        private void OnDisable()
        {
            _updateEventListener.ActionsToDo -= UpdateBehaviour;
        }


        private void UpdateBehaviour()
        {
        }


        private void Start()
        {
            _animatorLeg1 = GetComponent<Animator>();
            _animatorLeg2 = GetComponent<Animator>();
        }



        private void StartWalkingAnimation()
        {
            _animatorLeg1.SetBool("leg_is_go",true);
            _animatorLeg2.SetBool("leg2_is_go", true);
        }

        private void EndWalkingAnimation()
        {
            _animatorLeg1.SetBool("leg1_is_go", false);
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