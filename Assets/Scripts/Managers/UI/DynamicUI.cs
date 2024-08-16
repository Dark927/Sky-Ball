using UnityEngine;
using TMPro;

public class DynamicUI : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    public static DynamicUI Instance;

    [SerializeField] private TextMeshProUGUI _wavesCountText;
    [SerializeField] private TextMeshProUGUI _enemyCountText;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void UpdateWavesCount()
    {
        _wavesCountText.text = $"{WavesManager.Instance.CurrentWave:00}";
    }

    public void UpdateEnemyCount()
    {
        _enemyCountText.text = $"{WavesManager.Instance.AliveEnemiesCount:00}";
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

    #endregion
}
