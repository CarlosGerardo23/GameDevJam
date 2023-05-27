using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class TowerDefenseNetworkManager : NetworkManager
{
    [Header("Enemies Spawner")]
    [SerializeField] private GameObject _enemiesSpawnerPrefab;
    [SerializeField] private Transform[] _sapwnersPositions;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject enemiesSpawner = Instantiate(
            _enemiesSpawnerPrefab,
            _sapwnersPositions[numPlayers-1].position,
            _sapwnersPositions[numPlayers - 1].rotation);

        NetworkServer.Spawn(enemiesSpawner, conn);
    }
}
