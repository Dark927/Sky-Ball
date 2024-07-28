using UnityEngine;

public class EnemyRange : Enemy
{
    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Rockets Settings")]
    [Space]

    [SerializeField] private RocketLauncher _rocketLauncher;

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected override void Start()
    {
        base.Start();

        if (_rocketLauncher != null)
        {
            _rocketLauncher.StartRocketAttack<PlayerController>(transform);
        }
    }

    #endregion


}
