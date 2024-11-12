using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs = new List<GameObject>();
    [SerializeField]
    private List<GameObject> spawnPoints = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            int randomPrefabIndex = Random.Range(0, prefabs.Count);
            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Count);

            Instantiate(prefabs[randomPrefabIndex], spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
}
