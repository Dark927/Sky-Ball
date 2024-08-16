using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WavesManager : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    public static WavesManager Instance;
    public UnityEvent OnNextWaveStart = new();

    private int _currentWave = 0;
    private int _aliveEnemiesCount = 0;

    private List<WaveType> _waveTypes = new List<WaveType> { WaveType.Start, WaveType.Easy, WaveType.Medium, WaveType.Hard, WaveType.Boss };
    private List<int> _waveBounds = new List<int> { 2, 3, 6, 9, 11 };

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public int CurrentWave => _currentWave;
    public int AliveEnemiesCount => _aliveEnemiesCount;

    public WaveType CurrentWaveType
    {
        get
        {
            for (int i = 0; i < _waveBounds.Count; ++i)
            {
                if (_currentWave < _waveBounds[i])
                {
                    return _waveTypes[i];
                }
            }

            return WaveType.Hard;
        }
    }


    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods


    public void TryStartNextWave()
    {
        if (_aliveEnemiesCount == 0)
        {
            OnNextWaveStart.Invoke();
        }
    }

    public void DecrementAliveEnemiesCount()
    {
        _aliveEnemiesCount--;
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
            Destroy(Instance);
        }
    }

    private void Start()
    {
        EnemySpawner.Instance.EnemyPool.AddEventListenerToAll(TryStartNextWave);
        SetNewWaveEventListeners();

        Invoke(nameof(TryStartNextWave), 1f);
    }

    private void UpdateWaveInfo()
    {
        _currentWave += 1;
        _aliveEnemiesCount = EnemySpawner.Instance.EnemyPool.ActiveObjectsCount;
    }

    private void SetNewWaveEventListeners()
    {
        // Order is important, update methods must be called after the others. 

        OnNextWaveStart.AddListener(DifficultyManager.Instance.TryUpdateDifficultySettings);
        OnNextWaveStart.AddListener(EnemySpawner.Instance.SpawnNewEnemies);
        OnNextWaveStart.AddListener(PowersSpawner.Instance.SpawnNewPowers);

        OnNextWaveStart.AddListener(UpdateWaveInfo);  
        OnNextWaveStart.AddListener(DynamicUI.Instance.UpdateWavesCount);
        OnNextWaveStart.AddListener(DynamicUI.Instance.UpdateEnemyCount);
    }

    #endregion

}
