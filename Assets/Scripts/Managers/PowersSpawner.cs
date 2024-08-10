using System.Collections.Generic;
using UnityEngine;

public class PowersSpawner : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    public static PowersSpawner Instance;

    [Header("Main Settings")]
    [Space]

    [SerializeField] private PowerUpPool _powersPool;
    [SerializeField] private int _maxCount = 5;
    private int _countToSpawn;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public PowerUpPool PowersPool => _powersPool;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void SpawnNewPowers()
    {
        _countToSpawn = (DifficultyManager.Instance.PowerSpawnCount > _maxCount) ? _maxCount : DifficultyManager.Instance.PowerSpawnCount;

        _powersPool.DeactivateAll();

        for (int i = 0; i < _countToSpawn; ++i)
        {
            SpawnPowerUp();
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        SetInstance();
        ConfigureReferences();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ConfigureReferences()
    {
        if (_powersPool == null)
        {
            string errorMsg = $"{nameof(_powersPool)} == null, can not spawn powers! - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg, true);
        }
    }

    private void SpawnPowerUp()
    {
        Vector3 spawnPosition = PositionManager.Instance.RandomSpawnPosition<PowerUp>();

        if (spawnPosition.y > PositionManager.Instance.ErrorBoundY)
        {
            List<PowerUp.TYPE> availableTypes = _powersPool.AvailablePowerTypes;
            int randomPowerIndex = Random.Range(0, availableTypes.Count);

            PowerUp powerToSpawn = _powersPool.RequestInactivePower(availableTypes[randomPowerIndex], true);
            powerToSpawn.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            powerToSpawn.gameObject.SetActive(true);
        }
    }

    #endregion
}
