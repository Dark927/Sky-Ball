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

    private int _enemiesToSpawn = 2;
    private int _waveCount = 0;
    private int _increaseEnemyCountInterval = 3;

    private List<WaveType> _waveTypes = new List<WaveType> { WaveType.Start, WaveType.Easy, WaveType.Medium, WaveType.Hard, WaveType.Boss };
    private List<int> _waveBounds = new List<int> { 2, 3, 6, 9, 11 };

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private WaveType GetWaveType()
    {
        for (int i = 0; i < _waveBounds.Count; ++i)
        {
            if(_waveCount < _waveBounds[i])
            {
                return _waveTypes[i];
            }
        }

        return WaveType.Hard;
    }

    private List<GameObject> GenerateAvailableEnemyList(List<GameObject> allEnemyPrefabs, List<EnemyDefaultHead.TYPE> enemyTypesToSpawn)
    {
        List<GameObject> availableEnemyList = new();

        // Check every enemy prefab from available prefabs list

        foreach (GameObject enemyPrefab in allEnemyPrefabs)
        {
            if (CompareEnemyByType(enemyPrefab, enemyTypesToSpawn))
            {
                availableEnemyList.Add(enemyPrefab);
            }
        }

        return availableEnemyList;
    }

    private bool CompareEnemyByType(GameObject enemyPrefab, List<EnemyDefaultHead.TYPE> enemyTypes)
    {
        // Get enemy type 

        EnemyDefaultHead.TYPE type = CheckEnemyType(enemyPrefab);

        // Compare enemy type with available enemy types 

        foreach (EnemyDefaultHead.TYPE enemyType in enemyTypes)
        {
            if (type == enemyType)
            {
                return true;
            }
        }

        return false;
    }

    private EnemyDefaultHead.TYPE CheckEnemyType(GameObject enemyPrefab)
    {
        // Try get enemy component from prefab 

        EnemyDefaultHead enemy = enemyPrefab.GetComponent<EnemyDefaultHead>();

        if (enemy == null)
        {
            enemy = enemyPrefab.GetComponentInChildren<EnemyDefaultHead>();
        }

        // Return enemy type 

        if (enemy != null)
        {
            return enemy.Type;
        }

        return EnemyDefaultHead.TYPE.Default;
    }

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
        }
    }

    public int EnemySpawnCount
    {
        get { return _enemiesToSpawn; }
        set { return; }
    }

    public List<GameObject> AvailableEnemyList(List<GameObject> allEnemyPrefabs)
    {
        WaveType type = GetWaveType();

        switch (type)
        {
            default:
            case WaveType.Start:
                {
                    List<EnemyDefaultHead.TYPE> enemyTypesToSpawn = new List<EnemyDefaultHead.TYPE> { EnemyDefaultHead.TYPE.Default };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }

            case WaveType.Easy:
                {
                    List<EnemyDefaultHead.TYPE> enemyTypesToSpawn = new List<EnemyDefaultHead.TYPE> { EnemyDefaultHead.TYPE.Default, EnemyDefaultHead.TYPE.Fast, EnemyDefaultHead.TYPE.Range };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
            case WaveType.Medium:
                {
                    List<EnemyDefaultHead.TYPE> enemyTypesToSpawn = new List<EnemyDefaultHead.TYPE> { EnemyDefaultHead.TYPE.Default, EnemyDefaultHead.TYPE.Fast, EnemyDefaultHead.TYPE.Heavy };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
            case WaveType.Hard:
                {
                    List<EnemyDefaultHead.TYPE> enemyTypesToSpawn = new List<EnemyDefaultHead.TYPE> { EnemyDefaultHead.TYPE.Fast, EnemyDefaultHead.TYPE.Heavy, EnemyDefaultHead.TYPE.Group, EnemyDefaultHead.TYPE.Range };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
            case WaveType.Boss:
                {
                    List<EnemyDefaultHead.TYPE> enemyTypesToSpawn = new List<EnemyDefaultHead.TYPE> { EnemyDefaultHead.TYPE.Boss };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
        }
    }


    #endregion
}
