using UnityEngine;

public class RocketLauncherBody : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [Header("Main Settings")]
    [Space]

    [SerializeField] private RocketLogic.OWNER _lookTargetType;

    private GameObject _lookTarget;

    #endregion

    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void UpdateRotation()
    {
        if (_lookTarget == null) return;

        transform.LookAt(_lookTarget.transform);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        SetLookTarget();
    }

    private void SetLookTarget()
    {
        switch (_lookTargetType)
        {
            case RocketLogic.OWNER.Player:
                {
                    _lookTarget = FindObjectOfType<PlayerController>().gameObject;
                }
                break;

            case RocketLogic.OWNER.Enemy:
                {
                    _lookTarget = FindObjectOfType<EnemyBody>().gameObject;
                }
                break;
        }
    }

    #endregion
}
