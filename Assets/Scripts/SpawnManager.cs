using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    bool isSpawn = true;


    // Update is called once per frame
    void Update()
    {
        if(isSpawn)
        {
            SpawnEnemy(enemyPrefab);

            isSpawn = false;
        }
    }

    private void SpawnEnemy(GameObject enemyToSpawn)
    {
        SpawnPoint[] spawners = GetComponentsInChildren<SpawnPoint>();
        SpawnPoint actualSpawner = FindFarthestSpawner(spawners);
        actualSpawner.Spawn(enemyToSpawn);
    }

    private static SpawnPoint FindFarthestSpawner(SpawnPoint[] spawners)
    {
        SpawnPoint actualSpawner = null;
        float maxDistance = 0;

        foreach (SpawnPoint spawner in spawners)
        {
            if (spawner.IsBlocked())
            {
                continue;
            }

            Vector3 spawnerPosition = spawner.gameObject.transform.position;
            Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;

            float distanceToPlayer = (playerPosition - spawnerPosition).magnitude;

            if (distanceToPlayer > maxDistance)
            {
                maxDistance = distanceToPlayer;
                actualSpawner = spawner;
            }
        }

        return actualSpawner;
    }
}
