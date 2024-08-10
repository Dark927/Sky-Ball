using UnityEngine;
using UnityEngine.Events;

public class WavesManager : MonoBehaviour
{
    public static WavesManager Instance;
    public UnityEvent OnNextWaveStart = new();


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void TryStartNextWave()
    {
        if (EnemySpawner.Instance.EnemyPool.ActiveObjectsCount == 0)
        {
            OnNextWaveStart.Invoke();
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
            Destroy(Instance);
        }
    }

    private void Start()
    {
        EnemySpawner.Instance.EnemyPool.AddEventListenerToAll(TryStartNextWave);
        SetNewWaveEventListeners();

        Invoke(nameof(TryStartNextWave), 1f);
    }

    private void SetNewWaveEventListeners()
    {
        OnNextWaveStart.AddListener(DifficultyManager.Instance.NextWave);
        OnNextWaveStart.AddListener(EnemySpawner.Instance.SpawnNewEnemies);
        OnNextWaveStart.AddListener(PowersSpawner.Instance.SpawnNewPowers);
    }

    #endregion

}
