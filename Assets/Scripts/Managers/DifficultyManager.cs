using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    Start = 0,
    Easy = 1,
    Medium = 2,
    Hard = 3,

    Boss,

    FirstIndex = 0,
    LastIndex = Boss,
    NumberOfWaves = LastIndex + 1,
}

public class DifficultyManager : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    public static DifficultyManager Instance;

    private int _enemiesToSpawn = 2;
    private int _powersToSpawn = 1;
    private int _waveCount = 0;
    private int _increaseEnemyCountInterval = 3;

    private List<WaveType> _waveTypes = new List<WaveType> { WaveType.Start, WaveType.Easy, WaveType.Medium, WaveType.Hard, WaveType.Boss };
    private List<int> _waveBounds = new List<int> { 2, 3, 6, 9, 11 };

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public int EnemySpawnCount => _enemiesToSpawn;
    public int PowerSpawnCount => _powersToSpawn;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void NextWave()
    {
        _waveCount += 1;

        if ((_waveCount % _increaseEnemyCountInterval) == 0)
        {
            _enemiesToSpawn++;
            _powersToSpawn++;
        }
    }

    public List<EnemyHead.TYPE> AvailableEnemyTypes(EnemyPool pool)
    {
        WaveType type = GetWaveType();

        switch (type)
        {
            default:
            case WaveType.Start:
                {
                    List<EnemyHead.TYPE> enemyTypesToSpawn = new() { EnemyHead.TYPE.Default };
                    return GetWaveEnemyTypes(pool, enemyTypesToSpawn);
                }

            case WaveType.Easy:
                {
                    List<EnemyHead.TYPE> enemyTypesToSpawn = new() { EnemyHead.TYPE.Default, EnemyHead.TYPE.Fast, EnemyHead.TYPE.Range };
                    return GetWaveEnemyTypes(pool, enemyTypesToSpawn);
                }

            case WaveType.Medium:
                {
                    List<EnemyHead.TYPE> enemyTypesToSpawn = new() { EnemyHead.TYPE.Default, EnemyHead.TYPE.Fast, EnemyHead.TYPE.Heavy };
                    return GetWaveEnemyTypes(pool, enemyTypesToSpawn);
                }

            case WaveType.Hard:
                {
                    List<EnemyHead.TYPE> enemyTypesToSpawn = new() { EnemyHead.TYPE.Fast, EnemyHead.TYPE.Heavy, EnemyHead.TYPE.Group, EnemyHead.TYPE.Range };
                    return GetWaveEnemyTypes(pool, enemyTypesToSpawn);
                }

            case WaveType.Boss:
                {
                    List<EnemyHead.TYPE> enemyTypesToSpawn = new() { EnemyHead.TYPE.Boss };
                    return GetWaveEnemyTypes(pool, enemyTypesToSpawn);
                }
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

    private WaveType GetWaveType()
    {
        for (int i = 0; i < _waveBounds.Count; ++i)
        {
            if (_waveCount < _waveBounds[i])
            {
                return _waveTypes[i];
            }
        }

        return WaveType.Hard;
    }

    private List<EnemyHead.TYPE> GetWaveEnemyTypes(EnemyPool pool, List<EnemyHead.TYPE> enemyTypesToSpawn)
    {
        List<EnemyHead.TYPE> waveEnemyTypes = new();
        List<EnemyHead.TYPE> poolAvailableTypes = pool.RequestAvailableEnemyTypes();

        foreach (EnemyHead.TYPE currentType in poolAvailableTypes)
        {
            if (IsEnemyTypeInList(enemyTypesToSpawn, currentType))
            {
                waveEnemyTypes.Add(currentType);
            }
        }

        return waveEnemyTypes;
    }

    private bool IsEnemyTypeInList(List<EnemyHead.TYPE> sourceTypesList, EnemyHead.TYPE targetType)
    {
        foreach (EnemyHead.TYPE sourceType in sourceTypesList)
        {
            if (targetType == sourceType)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

}
