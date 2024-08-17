using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonBase<EnemySpawner>
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [Header("Enemy Settings")]
    [Space]

    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private int _maxCount = 500;
    private int _countToSpawn;

    private List<SpawnPoint> _allSpawnersList;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public EnemyPool EnemyPool => _enemyPool;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void SpawnNewEnemies()
    {
        foreach (SpawnPoint spawnPoint in _allSpawnersList)
        {
            spawnPoint.ResetState();
        }

        _countToSpawn = (DifficultyManager.Instance.EnemySpawnCount > _maxCount) ? _maxCount : DifficultyManager.Instance.EnemySpawnCount;

        List<EnemyHead.TYPE> availableTypes = DifficultyManager.Instance.AvailableEnemyTypes(_enemyPool);

        if (availableTypes.Count != 0)
        {
            for (int currentEnemy = 0; currentEnemy < _countToSpawn; ++currentEnemy)
            {
                int randomEnemyIndex = Random.Range(0, availableTypes.Count);
                EnemyHead enemyToSpawn = _enemyPool.RequestInactiveEnemy(availableTypes[randomEnemyIndex]);
                SpawnEnemy(enemyToSpawn);
            }
        }
        else
        {
            string warningMsg = $"Empty wave, list of available enemies is empty! - {gameObject.name}";
            ErrorsManager.Instance.SendWarningMsg(warningMsg);
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();
        ConfigureReferences();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void ConfigureReferences()
    {
        _allSpawnersList = new(GetComponentsInChildren<SpawnPoint>());

        if (_enemyPool == null)
        {
            string errorMsg = $"{nameof(_enemyPool)} == null, can not spawn enemies! - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg, true);
        }
    }

    private void SpawnEnemy(EnemyHead enemyToSpawn)
    {
        if(enemyToSpawn == null)
        {
            return;
        }

        SpawnPoint availableSpawner = PositionManager.Instance.FindFarthestSpawner(_allSpawnersList);

        if (availableSpawner != null)
        {
            availableSpawner.Spawn(enemyToSpawn);
        }
        else
        {
            // Generate random accessible position if all spawners are blocked 

            Vector3 targetSpawnPosition = PositionManager.Instance.RandomSpawnPosition<EnemyHead>();

            if (targetSpawnPosition.y > PositionManager.Instance.ErrorBoundY)
            {
                enemyToSpawn.transform.position = targetSpawnPosition;
                enemyToSpawn.gameObject.SetActive(true);
            }
        }

        enemyToSpawn.TryActivatePowers();
    }

    #endregion
}
