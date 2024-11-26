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
    private List<WeaponSO> weapons = new List<WeaponSO>();

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private float spawnTime;
    public void StartSpawning(int playerCount)
    {
        StartCoroutine(SpawnObjects(playerCount));
    }

    private IEnumerator SpawnObjects(int playerCount)
    {
        while (true)
        {
            spawnTime = Random.Range(2f, 6f);
            if (playerCount > 2)
            {
                spawnTime = Random.Range(1.5f, 4f);
            }
            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Count);

            switch (gameManager.gamePhase)
            {
                case GameManager.GamePhase.Preparation:
                    int randomPrefabprepIndex = Random.Range(0, prefabsPreparationPhase.Count);
                    Instantiate(prefabsPreparationPhase[randomPrefabprepIndex], spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(spawnTime);
                    break;

                case GameManager.GamePhase.Action:

                    int randomPrefabActionPhaseIndex = Random.Range(0, prefabsActionPhase.Count);
                    int randomPrefabWeaponIndex = Random.Range(0, weapons.Count);
                    Instantiate(prefabsActionPhase[randomPrefabActionPhaseIndex], spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(spawnTime);
                    Instantiate(weapons[randomPrefabWeaponIndex].weaponPrefab, spawnPoints[randomSpawnPointIndex].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(spawnTime);
                    break;

                case GameManager.GamePhase.PostAction:
                    yield return null;
                    break;
            }
        }
    }

}
