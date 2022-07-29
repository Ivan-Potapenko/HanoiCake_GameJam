using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public class MusicalController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSorceForFall;

        [SerializeField]
        private AudioSource _audioSorceForJump;

        private CharacterController _characterController;
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _characterController.onGround += UpdateSoundForFall;
            _characterController.isJump += UpdateSoundForJump;
        }

        private void OnDisable()
        {
            _characterController.onGround -= UpdateSoundForFall;
            _characterController.isJump -= UpdateSoundForJump;
        }

        private void UpdateSoundForFall()
        {
            _audioSorceForFall.Play();
        }

        private void UpdateSoundForJump()
        {
            _audioSorceForJump.Play();
        }
    }
}