using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Main Settings")]
    [Space]

    [SerializeField] private float _reloadTime = 0.5f;
    [SerializeField] private Transform _shootPoint;
    protected ObjectsPool _rocketsPool;


    [Header("VFX optional Settings")]
    [Space]

    [SerializeField] private GameObject _muzzleVFX;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties


    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 

    public void StartRocketAttack<Target>() where Target : MonoBehaviour
    {
        StartCoroutine(LaunchRocketsRoutine<Target>());
    }

    public virtual void LaunchRocket(Transform target)
    {
        Vector3 lookDirection = (target.position - _shootPoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        // Get rocket from pool and configure it before activation 

        GameObject rocket = _rocketsPool.RequestInactiveObject(true);
        rocket.transform.SetPositionAndRotation(_shootPoint.position, lookRotation);

        RocketLogic rocketLogic = rocket.GetComponent<RocketLogic>();

        if (rocketLogic != null)
        {
            rocketLogic.SetTarget(target);
        }
        else
        {
            string errorMsg = $"{nameof(rocketLogic)} is null, can not set target. - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg);
        }

        rocket.SetActive(true);

        ActivateVFX();
    }

    public void StopRocketAttack()
    {
        StopAllCoroutines();
    }

    public void UpdateShootPointPosition(Vector3 position)
    {
        position.y = _shootPoint.position.y;
        _shootPoint.position = position;
    }

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    protected virtual void Awake()
    {
        _rocketsPool = GetComponent<ObjectsPool>();
    }

    private IEnumerator LaunchRocketsRoutine<Target>() where Target : MonoBehaviour
    {
        while (true)
        {
            Target[] activeTargetsList = FindObjectsOfType<Target>();

            foreach (Target target in activeTargetsList)
            {
                LaunchRocket(target.transform);
            }

            yield return new WaitForSeconds(_reloadTime);
        }
    }

    private void ActivateVFX()
    {
        if (_muzzleVFX != null)
        {
            _muzzleVFX.SetActive(true);
        }
    }

    #endregion

}
