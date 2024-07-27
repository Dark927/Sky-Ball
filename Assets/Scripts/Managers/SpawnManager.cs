using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [Header("Enemy Settings")]
    [Space]

    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private int _maxEnemiesCount = 500;
    private int _activeEnemies = 0;

    [Space]
    [Header("Power Ups Settings")]
    [Space]

    [SerializeField] private List<GameObject> _powerUpPrefabs;
    [SerializeField] private int _maxPowerUpsCount = 5;
    [SerializeField] private int _powerUpsToSpawn = 1;


    [Space]
    [Header("Random position Settings")]
    [Space]

    [SerializeField] private float _safeDistance = 3f;
    [SerializeField] private float _spawnRadius = 8f;


    private List<SpawnPoint> _actualSpawners = new();
    private DifficultyManager _difficultyManager;

    // Error identifiers 

    private float _errorBoundY = -50f;
    private bool _fatalError = false;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _difficultyManager = FindObjectOfType<DifficultyManager>();

        if(_difficultyManager == null)
        {
            _fatalError = true;
            Debug.Log("# Fatal Error : SpawnManager.cs -> difficultyManager == null, can't spawn enemies.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_fatalError)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        _activeEnemies = FindObjectsOfType<Enemy>().Length;

        if (_activeEnemies == 0)
        {
            _difficultyManager.NextWave();

            SpawnNewWave(_difficultyManager.EnemySpawnCount);
        }
    }

    private void SpawnNewWave(int enemiesCount)
    {
        // ------------------------------------------
        // Destroy previous Power Ups and spawn New ones
        // ------------------------------------------

        // Check if powerUpsCount is out of limit 

        if (_powerUpsToSpawn > _maxPowerUpsCount)
        {
            _powerUpsToSpawn = _maxPowerUpsCount;
        }

        DestroyActivePowerUps();

        // Spawn new power ups

        for (int i = 0; i < _powerUpsToSpawn; ++i)
        {
            SpawnPowerUp();
        }


        // ------------------------------------------
        // Enemy spawn
        // ------------------------------------------

        // Check if enemiesCount is out of limit 

        if (enemiesCount > _maxEnemiesCount)
        {
            enemiesCount = _maxEnemiesCount;
        }

        List<GameObject> availableEnemies = _difficultyManager.AvailableEnemyList(_enemyPrefabs);

        // Spawn enemies in cycle 
        for (int i = 0; i < enemiesCount; ++i)
        {
            if (availableEnemies.Count == 0)
            {
                Debug.Log("# Warning :: Skip wave, list of available enemies is empty! - " + gameObject.name);
                break;
            }

            int enemyIndex = Random.Range(0, availableEnemies.Count);

            SpawnEnemy(availableEnemies[enemyIndex]);
        }

        _actualSpawners.Clear();
    }

    private void DestroyActivePowerUps()
    {
        PowerUp[] powerUps = FindObjectsOfType<PowerUp>();

        foreach (PowerUp power in powerUps)
        {
            Destroy(power.gameObject);
        }
    }

    private void SpawnPowerUp()
    {
        int powerRandomIndex = Random.Range(0, _powerUpPrefabs.Count);
        Vector3 powerSpawnPosition = CalculateRandomSpawnPosition<PowerUp>(_powerUpPrefabs[powerRandomIndex]);

        if (powerSpawnPosition.y > _errorBoundY)
        {
            Instantiate(_powerUpPrefabs[powerRandomIndex], powerSpawnPosition, _powerUpPrefabs[powerRandomIndex].transform.rotation);
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

            if (actualSpawnPosition.y > _errorBoundY)
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
        // Fields for generating spawn point
        // ------------------------------------------

        Vector3 actualSpawnPosition;
        bool isCorrectSpawnPoint;

        int maxAttempts = 1000;


        // ------------------------------------------
        // Generating position cycle 
        // ------------------------------------------

        do
        {
            Vector2 positionInCircle = Random.insideUnitCircle * _spawnRadius;
            actualSpawnPosition = new Vector3(positionInCircle.x, objectToSpawn.transform.position.y, positionInCircle.y);

            isCorrectSpawnPoint = true;
            maxAttempts--;

            // Check distance between generated position and each entity
            foreach (Vector3 entityPosition in entitiesPositions)
            {
                float distanceToEntity = (entityPosition - actualSpawnPosition).magnitude;

                if (distanceToEntity < _safeDistance)
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
            if (spawner.IsBlocked() || _actualSpawners.Contains(spawner))
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

        _actualSpawners.Add(actualSpawner);
        return actualSpawner;
    }

    #endregion
}
