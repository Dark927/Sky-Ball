using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    public static SpawnManager instance;

    [Header("Enemy Settings")]
    [Space]

    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private int _maxEnemiesCount = 500;

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

    private List<SpawnPoint> _allSpawnersList;
    private DifficultyManager _difficultyManager;
    private Transform _playerTransform;

    // Error identifiers 

    private float _errorBoundY = -50f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ConfigureReferences();
    }

    private void ConfigureReferences()
    {
        _difficultyManager = FindObjectOfType<DifficultyManager>();
        _allSpawnersList = new(GetComponentsInChildren<SpawnPoint>());
        PlayerController player = FindObjectOfType<PlayerController>();


        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            string errorMsg = $"{player} == null, can not find Player object! - {gameObject.name}";
            ErrorsManager.instance.SendErrorMsg(errorMsg, true);
        }

        if ((_difficultyManager == null) || (_enemyPool == null))
        {
            string nullReferenceObjectName = (_difficultyManager != null) ? nameof(_enemyPool) : nameof(_difficultyManager);

            string errorMsg = $"{nullReferenceObjectName} == null, can not spawn enemies! - {gameObject.name}";
            ErrorsManager.instance.SendErrorMsg(errorMsg, true);
        }
    }

    private void Start()
    {
        _enemyPool.AddEventListenerToAll(TryStartNextWave);
        Invoke(nameof(TryStartNextWave), 1f);
    }


    public void TryStartNextWave()
    {
        if ((_enemyPool.ActiveEnemiesCount == 0))
        {
            _difficultyManager.NextWave();
            SpawnNewWave(_difficultyManager.EnemySpawnCount);
        }
    }

    private void SpawnNewWave(int enemiesCount)
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Destroy previous Power Ups and spawn new ones
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Check if power ups count is out of limit 

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


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Enemy spawn
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Reset all spawners

        foreach(SpawnPoint spawnPoint in _allSpawnersList)
        {
            spawnPoint.ResetState();
        }

        // Check if enemies count is out of limit 

        if (enemiesCount > _maxEnemiesCount)
        {
            enemiesCount = _maxEnemiesCount;
        }

        List<EnemyHead.TYPE> availableEnemyTypes = _difficultyManager.AvailableEnemyTypes(_enemyPool);

        // Spawn enemies

        if (availableEnemyTypes.Count != 0)
        {
            for (int currentEnemy = 0; currentEnemy < enemiesCount; ++currentEnemy)
            {
                int enemyTypeIndex = Random.Range(0, availableEnemyTypes.Count);
                EnemyHead enemyToSpawn = _enemyPool.RequestInactiveEnemy(availableEnemyTypes[enemyTypeIndex]);
                Debug.Log(enemyToSpawn);
                SpawnEnemy(enemyToSpawn);
            }
        }
        else
        {
            string warningMsg = $"Skip wave, list of available enemies is empty! - {gameObject.name}";
            ErrorsManager.instance.SendWarningMsg(warningMsg);
        }
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

    private void SpawnEnemy(EnemyHead enemyToSpawn)
    {
        SpawnPoint availableSpawner = FindFarthestSpawner();

        if (availableSpawner != null)
        {
            availableSpawner.Spawn(enemyToSpawn);
        }
        else
        {
            // Generate random accessible position if all spawners are blocked 

            Vector3 targetSpawnPosition = CalculateRandomSpawnPosition<EnemyHead>(enemyToSpawn.gameObject);

            if (targetSpawnPosition.y > _errorBoundY)
            {
                enemyToSpawn.transform.position = targetSpawnPosition;
                enemyToSpawn.gameObject.SetActive(true);
            }
        }
    }


    private Vector3 CalculateRandomSpawnPosition<Entity>(GameObject objectToSpawn) where Entity : MonoBehaviour
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Collect all active entities positions 
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        List<Vector3> entitiesPositions = new() { _playerTransform.position };

        Entity[] activeEntities = FindObjectsOfType<Entity>();

        foreach (Entity entity in activeEntities)
        {
            entitiesPositions.Add(entity.transform.position);
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Fields for generating random spawn point
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        Vector3 actualSpawnPosition;
        bool isCorrectSpawnPoint;

        int maxAttempts = 100;


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Generating position cycle 
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        do
        {
            Vector2 positionInCircle = Random.insideUnitCircle * _spawnRadius;
            actualSpawnPosition = new Vector3(positionInCircle.x, 0, positionInCircle.y);

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
            actualSpawnPosition = new Vector3(0, _errorBoundY, 0);
        }

        return actualSpawnPosition;
    }


    private SpawnPoint FindFarthestSpawner()
    {
        SpawnPoint farthestSpawner = null;
        float sqrMaxDistance = 0;

        foreach (SpawnPoint spawner in _allSpawnersList)
        {
            if (spawner.IsBlocked)
            {
                continue;
            }

            Vector3 spawnerPosition = spawner.transform.position;
            Vector3 playerPosition = _playerTransform.position;

            float sqrDistanceToPlayer = (playerPosition - spawnerPosition).sqrMagnitude;

            if (sqrDistanceToPlayer > sqrMaxDistance)
            {
                sqrMaxDistance = sqrDistanceToPlayer;
                farthestSpawner = spawner;
            }
        }

        farthestSpawner.PrepareToSpawn();
        return farthestSpawner;
    }

    #endregion
}
