using UnityEngine;
using TMPro;

public class UIGameplay : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private TextMeshProUGUI _wavesCountText;
    [SerializeField] private TextMeshProUGUI _enemyCountText;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void UpdateSessionInfo()
    {
        UpdateWavesCount();
        UpdateAliveEnemiesCount();
    }

    public void UpdateWavesCount()
    {
        _wavesCountText.text = $"{WavesManager.Instance.CurrentWave:00}";
    }

    public void UpdateAliveEnemiesCount()
    {
        _enemyCountText.text = $"{WavesManager.Instance.AliveEnemiesCount:00}";
    }


    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods




    #endregion
}
