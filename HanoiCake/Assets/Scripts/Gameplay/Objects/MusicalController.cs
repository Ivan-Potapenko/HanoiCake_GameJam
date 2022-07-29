using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public class MusicalController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSorceForFall;

        private CharacterController _characterController;
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _characterController.onGround += UpdateSound;
        }

        private void OnDisable()
        {
            _characterController.onGround -= UpdateSound;
        }

        private void UpdateSound()
        {
            _audioSorceForFall.Play();
        }
    }
}