using System.Collections;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Main Settings")]
    [Space]

    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private float _reloadTime = 0.5f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private IEnumerator LaunchRocketsRoutine<Target>() where Target : MonoBehaviour
    {
        while (true)
        {
            Target[] activeTargetsList = FindObjectsOfType<Target>();

            foreach (Target target in activeTargetsList)
            {
                LaunchRocket(transform, target.transform);
            }

            yield return new WaitForSeconds(_reloadTime);
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 

    public void StartRocketAttack<Target>() where Target : MonoBehaviour
    {
        StartCoroutine(LaunchRocketsRoutine<Target>());
    }

    public void LaunchRocket(Transform source, Transform target)
    {
        Vector3 lookDirection = (target.position - source.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        GameObject rocket = Instantiate(_rocketPrefab, source.position + Vector3.up, lookRotation);
        rocket.GetComponent<RocketLogic>().SetTarget(target);
    }

    public void StopRocketAttack()
    {
        StopAllCoroutines();
    }

    #endregion
}
