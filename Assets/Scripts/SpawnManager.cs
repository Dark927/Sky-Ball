using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Main Settings")]

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemiesCount = 500;
    [SerializeField] int enemiesCount = 1;
    int activeEnemies = 0;

    [Space]
    [Header("Random position Settings")]

    [SerializeField] float safeDistance = 3f;
    [SerializeField] float spawnRadius = 8f;

    List<SpawnPoint> actualSpawners = new();


    // Update is called once per frame
    void Update()
    {
        activeEnemies = FindObjectsOfType<Enemy>().Length;

        if (activeEnemies == 0)
        {
            SpawnNewWave(enemiesCount);
        }
    }

    private void SpawnNewWave(int enemiesCount)
    {
        // Check if enemiesCount is out of limit 
        if (enemiesCount > maxEnemiesCount)
        {
            enemiesCount = maxEnemiesCount;
        }

        // Spawn enemies in cycle 
        for (int i = 0; i < enemiesCount; ++i)
        {
            SpawnEnemy(enemyPrefab);
        }

        actualSpawners.Clear();
    }

    private void SpawnEnemy(GameObject enemyToSpawn)
    {
        SpawnPoint[] spawners = GetComponentsInChildren<SpawnPoint>();
        SpawnPoint actualSpawner = FindFarthestSpawner(spawners);

        // Use spawner if it is not blocked 
        if (actualSpawner != null)
        {
            actualSpawner.Spawn(enemyToSpawn);
        }

        // Generate random accessible position if all spawners are blocked 
        else
        {
            Vector3 actualSpawnPosition = CalculateRandomSpawnPosition(enemyToSpawn);
            Instantiate(enemyToSpawn, actualSpawnPosition, Quaternion.identity);
        }

    }

    private Vector3 CalculateRandomSpawnPosition(GameObject enemyToSpawn)
    {
        // ------------------------------------------
        // Collect all active entities positions 
        // ------------------------------------------

        Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;

        List<Vector3> entitiesPositions = new();
        entitiesPositions.Add(playerPosition);

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            entitiesPositions.Add(enemy.transform.position);
        }


        // ------------------------------------------
        // Parameters for generating spawn point
        // ------------------------------------------

        Vector3 actualSpawnPosition = Vector3.zero;
        bool isCorrectSpawnPoint = true;

        int maxAttempts = 1000;


        // ------------------------------------------
        // Generating position cycle 
        // ------------------------------------------

        do
        {
            Vector2 positionInCircle = Random.insideUnitCircle * spawnRadius;
            actualSpawnPosition = new Vector3(positionInCircle.x, enemyToSpawn.transform.position.y, positionInCircle.y);

            isCorrectSpawnPoint = true;
            maxAttempts--;

            // Check distance between generated position and each entity
            foreach (Vector3 entityPosition in entitiesPositions)
            {
                float distanceToEntity = (entityPosition - actualSpawnPosition).magnitude;

                if (distanceToEntity < safeDistance)
                {
                    isCorrectSpawnPoint = false;
                    break;
                }
            }
        } while (!isCorrectSpawnPoint && (maxAttempts > 0));

        if(maxAttempts == 0)
        {
            actualSpawnPosition = new Vector3(0, -100f, 0);
        }

        return actualSpawnPosition;
    }


    private SpawnPoint FindFarthestSpawner(SpawnPoint[] spawners)
    {
        SpawnPoint actualSpawner = null;
        float maxDistance = 0;

        foreach (SpawnPoint spawner in spawners)
        {
            if (spawner.IsBlocked() || actualSpawners.Contains(spawner))
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

        actualSpawners.Add(actualSpawner);
        return actualSpawner;
    }
}
