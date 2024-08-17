using TMPro;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private TextMeshProUGUI _wavesCountText;
    [SerializeField] private TextMeshProUGUI _enemyKilledText;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void UpdateSessionInfo()
    {
        UpdateCurrentWaveInfo();
        UpdateSessionKillsInfo();
    }

    public void UpdateCurrentWaveInfo()
    {
        _wavesCountText.text = $"{WavesManager.Instance.CurrentWave:00}";
    }

    public void UpdateSessionKillsInfo()
    {
        _enemyKilledText.text = $"{GameManager.Instance.SessionKills:00}";
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void OnEnable()
    {
        UpdateSessionInfo();
    }

    #endregion
}
