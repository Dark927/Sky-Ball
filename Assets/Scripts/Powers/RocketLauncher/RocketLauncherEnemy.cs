using System.Collections;
using UnityEngine;

public class RocketLauncherEnemy : RocketLauncher
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Space]
    [Header("Enemy Settings")]
    [Space]

    [SerializeField] private EnemyBody _enemyBody;
    [SerializeField] private float _verticalOffset = 0.7f;
    [SerializeField] private float _blockRotationDelay = 0.25f;

    private RocketLauncherBody _rocketLauncherBody;
    private bool _isBodyRotationBlocked = false;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 

    public override void LaunchRocket(Transform target)
    {
        base.LaunchRocket(target);
        StartCoroutine(BlockLauncherRotation(_blockRotationDelay));
    }

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    protected override void Awake()
    {
        base.Awake();
        _rocketLauncherBody = GetComponentInChildren<RocketLauncherBody>();
    }

    private void Update()
    {
        UpdateBodyPositionAndRotation();
    }

    private IEnumerator BlockLauncherRotation(float delay)
    {
        _isBodyRotationBlocked = true;
        yield return new WaitForSeconds(delay);
        _isBodyRotationBlocked = false;
    }

    private void UpdateBodyPositionAndRotation()
    {
        _rocketLauncherBody.transform.position = CalcRocketLauncherPosition();

        if (!_isBodyRotationBlocked)
        {
            _rocketLauncherBody.UpdateRotation();
        }
    }

    private Vector3 CalcRocketLauncherPosition()
    {
        return _enemyBody.transform.position + new Vector3(0f, _verticalOffset, 0f);
    }

    #endregion
}
