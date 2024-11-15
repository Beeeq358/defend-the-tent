using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabsPreparationPhase = new List<GameObject>();
    [SerializeField]
    private List<GameObject> prefabsActionPhase = new List<GameObject>();
    [SerializeField]
    private List<GameObject> spawnPoints = new List<GameObject>();

    [SerializeField]
    private GameManager gameManager;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Count);

        switch (gameManager.gamePhase)
        {
            case GameManager.GamePhase.Preparation:
                int randomPrefabprepIndex = Random.Range(0, prefabsPreparationPhase.Count);
                Instantiate(prefabsPreparationPhase[randomPrefabprepIndex], spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(2f);
                break;
            case GameManager.GamePhase.Action:
                int randomPrefabIndex = Random.Range(0, prefabsActionPhase.Count);
                Instantiate(prefabsActionPhase[randomPrefabIndex], spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(5f);
                break;
            case GameManager.GamePhase.PostAction:
                yield return null;
                break;
        }
    }
}
