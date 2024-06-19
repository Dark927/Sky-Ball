using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Settings")]

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemiesCount = 500;
    [SerializeField] int enemiesToSpawn = 1;
    int activeEnemies = 0;

    [Space]
    [Header("Power Ups Settings")]

    [SerializeField] List<GameObject> powerUpPrefabs;
    [SerializeField] int maxPowerUpsCount = 5;
    [SerializeField] int powerUpsToSpawn = 1;


    [Space]
    [Header("Random position Settings")]

    [SerializeField] float safeDistance = 3f;
    [SerializeField] float spawnRadius = 8f;
    float errorBoundY = -50f;

    List<SpawnPoint> actualSpawners = new();
    int waveCount = 0;
    int increaseDifficultyInterval = 3;


    // Update is called once per frame
    void Update()
    {
        activeEnemies = FindObjectsOfType<Enemy>().Length;

        if (activeEnemies == 0)
        {
            waveCount++;

            if ((waveCount % increaseDifficultyInterval) == 0)
            {
                enemiesToSpawn++;
            }

            SpawnNewWave(enemiesToSpawn);
        }
    }

    private void SpawnNewWave(int enemiesCount)
    {
        // ------------------------------------------
        // Destroy previous Power Ups and spawn New ones
        // ------------------------------------------

        // Check if powerUpsCount is out of limit 

        if(powerUpsToSpawn > maxPowerUpsCount)
        {
            powerUpsToSpawn = maxPowerUpsCount;
        }

        // Find and destroy old power ups 

        PowerUp[] powerUps = FindObjectsOfType<PowerUp>();

        foreach (PowerUp power in powerUps)
        {
            Destroy(power.gameObject);
        }

        // Spawn new power ups

        for (int i = 0; i < powerUpsToSpawn; ++i)
        {
            SpawnPowerUp();
        }


        // ------------------------------------------
        // Enemy spawn
        // ------------------------------------------

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

    private void SpawnPowerUp()
    {
        int powerRandomIndex = Random.Range(0, powerUpPrefabs.Count);
        Vector3 powerSpawnPosition = CalculateRandomSpawnPosition<PowerUp>(powerUpPrefabs[powerRandomIndex]);

        if (powerSpawnPosition.y > errorBoundY)
        {
            Instantiate(powerUpPrefabs[powerRandomIndex], powerSpawnPosition, Quaternion.identity);
        }
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
            Vector3 actualSpawnPosition = CalculateRandomSpawnPosition<Enemy>(enemyToSpawn);

            // Spawn object only at correct positions

            if (actualSpawnPosition.y > errorBoundY)
            {
                Instantiate(enemyToSpawn, actualSpawnPosition, Quaternion.identity);
            }
        }

    }


    private Vector3 CalculateRandomSpawnPosition<T>(GameObject objectToSpawn) where T : MonoBehaviour
    {
        // ------------------------------------------
        // Collect all active entities positions 
        // ------------------------------------------

        Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;

        List<Vector3> entitiesPositions = new();
        entitiesPositions.Add(playerPosition);

        T[] activeEntities = FindObjectsOfType<T>();

        foreach (T entity in activeEntities)
        {
            entitiesPositions.Add(entity.transform.position);
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
            actualSpawnPosition = new Vector3(positionInCircle.x, objectToSpawn.transform.position.y, positionInCircle.y);

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

        if (maxAttempts == 0)
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
