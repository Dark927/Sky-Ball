using UnityEngine;

public class EnemyPowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    [Space]
    [Header("Powers Settings")]

    [SerializeField] private RocketLauncher _rocketLauncher;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void TryActivateRocketLauncher()
    {
        if (_rocketLauncher != null)
        {
            _rocketLauncher.StartRocketAttack<PlayerMovement>();
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods



    #endregion

}
