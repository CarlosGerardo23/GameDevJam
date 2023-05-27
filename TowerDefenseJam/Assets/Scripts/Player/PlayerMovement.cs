using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Camera _mainCamera;

    #region Serevr
    [Command]
    private void CmdMove(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
    #endregion
    #region Client
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        _mainCamera = Camera.main;
    }
    [ClientCallback]
    private void Update()
    {
        if (!isOwned) return;
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                CmdMove(hit.point);
        }
    }
    #endregion
}
