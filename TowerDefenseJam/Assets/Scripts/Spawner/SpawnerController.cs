using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnerController : NetworkBehaviour
{
    [SerializeField] private GameObject _spawnablePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _numOfIntance;
    [SerializeField] private float _timeBetweenInstance;

    private void Start()
    {
        StartCoroutine(SpawnObject());
    }
    #region Server
    [Command]
    private void CmdSpwanObject()
    {
        GameObject unitIntsance = Instantiate(_spawnablePrefab, _spawnPoint.position, _spawnPoint.rotation);
        NetworkServer.Spawn(unitIntsance, connectionToClient);
    }
    #endregion
    #region Client

    private IEnumerator SpawnObject()
    {
        int currentSpawner = 0;
        if (!isOwned) yield break;
        do
        {
            yield return new WaitForSeconds(_timeBetweenInstance);
            CmdSpwanObject();
            currentSpawner++;

        } while (currentSpawner < _numOfIntance);
    }
    #endregion
}
