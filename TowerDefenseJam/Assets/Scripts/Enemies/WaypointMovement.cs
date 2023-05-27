using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class WaypointMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    public Transform[] onSceneWaypointsList;
    public Vector3[] waypointsPositionsList;
    [SyncVar(hook = nameof(HandleCurrentPoint))]
    private int _serverPoint = -1;
    private int _currentPoint = -1;
    private bool _isSet;


    [ClientCallback]
    private void Start()
    {
        if (!isOwned) return;
        _currentPoint = -1;
        _serverPoint = -1;
        _isSet = true;
    }
    #region Server
    [Command]
    private void CmdMove(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
    [Server]
    private void SetCurrentPoint(int currentPoint)
    {
        _serverPoint = currentPoint;
    }
    [Command]
    private void CmdUpdateCurrentPoint(int currentPoint)
    {
        SetCurrentPoint(currentPoint);
    }
    private void HandleCurrentPoint(int oldIndex, int newIndex)
    {
        if (!isOwned) return;
        _currentPoint = newIndex;
        if (_currentPoint < waypointsPositionsList.Length)
            CmdMove(waypointsPositionsList[_currentPoint]);
    }
    #endregion
    #region Client
    [ClientCallback]
    private void Update()
    {
        if (!isOwned && !_isSet) return;
        if (_currentPoint == -1)
        {

            CmdUpdateCurrentPoint(0);
        }
        else if (Vector3.Distance(transform.position,waypointsPositionsList[_currentPoint]) < _agent.stoppingDistance)
        {
            if (_currentPoint < waypointsPositionsList.Length)
            {

                CmdUpdateCurrentPoint(_currentPoint + 1);
            }
        }
    }
    #endregion
}
