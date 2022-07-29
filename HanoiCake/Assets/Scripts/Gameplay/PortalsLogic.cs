using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class PortalsLogic : MonoBehaviour
{
    [SerializeField]
    private Portal _topPortal;

    [SerializeField]
    private Portal _bottomPortal;

    [SerializeField]
    private EventListener _upadteEventListner;

    [SerializeField]
    private EventDispatcher _nextLevelEventDipatcher;

    private void OnEnable() {
        _upadteEventListner.ActionsToDo += UpdateBehaviour;
    }

    private void OnDisable() {
        _upadteEventListner.ActionsToDo -= UpdateBehaviour;
    }

    private void UpdateBehaviour() {
        if(_topPortal.IsActivated && _bottomPortal.IsActivated) {
            _nextLevelEventDipatcher.Dispatch();
        }
    }
}
