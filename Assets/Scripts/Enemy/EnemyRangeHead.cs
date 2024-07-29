using UnityEngine;

public class EnemyRangeHead : EnemyDefaultHead
{
    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    [Space]
    [Header("Powers Settings")]

    [SerializeField] private RocketLauncher _rocketLauncher;

    public EnemyBody Body => _body;


    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        _rocketLauncher.StartRocketAttack<PlayerController>();
    }

    #endregion


}
