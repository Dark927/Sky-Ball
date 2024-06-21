using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    Wave_start = 0,
    Wave_easy = 1,
    Wave_medium = 2,
    Wave_hard = 3,

    Wave_boss,

    Wave_firstIndex = 0,
    Wave_lastIndex = Wave_boss,
    Wave_numberOfWaves = Wave_lastIndex + 1,
}

public class DifficultyManager : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    List<GameObject> currentEnemyList = new();
    int enemiesToSpawn = 1;
    int waveCount = 0;
    int increaseDifficultyInterval = 3;

    List<WaveType> waveTypes = new List<WaveType> { WaveType.Wave_start, WaveType.Wave_easy, WaveType.Wave_medium, WaveType.Wave_hard, WaveType.Wave_boss };
    List<int> waveBounds = new List<int> { 2, 3, 6, 9, 11 };

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private WaveType GetWaveType()
    {
        for (int i = 0; i < waveBounds.Count; ++i)
        {
            if(waveCount < waveBounds[i])
            {
                return waveTypes[i];
            }
        }

        return WaveType.Wave_hard;
    }

    private List<GameObject> GenerateAvailableEnemyList(List<GameObject> allEnemyPrefabs, List<EnemyType> enemyTypesToSpawn)
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

    private bool CompareEnemyByType(GameObject enemyPrefab, List<EnemyType> enemyTypes)
    {
        // Get enemy type 

        EnemyType type = CheckEnemyType(enemyPrefab);

        // Compare enemy type with available enemy types 

        foreach (EnemyType enemyType in enemyTypes)
        {
            if (type == enemyType)
            {
                return true;
            }
        }

        return false;
    }

    private EnemyType CheckEnemyType(GameObject enemyPrefab)
    {
        // Try get enemy component from prefab 

        Enemy enemy = enemyPrefab.GetComponent<Enemy>();

        if (enemy == null)
        {
            enemy = enemyPrefab.GetComponentInChildren<Enemy>();
        }

        // Return enemy type 

        if (enemy != null)
        {
            return enemy.GetEnemyType();
        }

        return EnemyType.Enemy_default;
    }

    #endregion

    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void NextWave()
    {
        waveCount += 1;

        if ((waveCount % increaseDifficultyInterval) == 0)
        {
            enemiesToSpawn++;
        }
    }

    public int EnemySpawnCount
    {
        get { return enemiesToSpawn; }
        set { return; }
    }

    public List<GameObject> AvailableEnemyList(List<GameObject> allEnemyPrefabs)
    {
        WaveType type = GetWaveType();

        switch (type)
        {
            default:
            case WaveType.Wave_start:
                {
                    List<EnemyType> enemyTypesToSpawn = new List<EnemyType> { EnemyType.Enemy_default };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }

            case WaveType.Wave_easy:
                {
                    List<EnemyType> enemyTypesToSpawn = new List<EnemyType> { EnemyType.Enemy_default, EnemyType.Enemy_fast };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
            case WaveType.Wave_medium:
                {
                    List<EnemyType> enemyTypesToSpawn = new List<EnemyType> { EnemyType.Enemy_default, EnemyType.Enemy_fast, EnemyType.Enemy_powerful };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
            case WaveType.Wave_hard:
                {
                    List<EnemyType> enemyTypesToSpawn = new List<EnemyType> { EnemyType.Enemy_fast, EnemyType.Enemy_powerful, EnemyType.Enemy_group };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
            case WaveType.Wave_boss:
                {
                    List<EnemyType> enemyTypesToSpawn = new List<EnemyType> { EnemyType.Enemy_default, EnemyType.Enemy_fast, EnemyType.Enemy_boss };
                    return GenerateAvailableEnemyList(allEnemyPrefabs, enemyTypesToSpawn);
                }
        }
    }


    #endregion
}
