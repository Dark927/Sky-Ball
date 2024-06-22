using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    [Header("Main Settings")]
    [Space]

    [SerializeField] GameObject rocketPrefab;
    [SerializeField] float reloadTime = 0.5f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    IEnumerator LaunchRocketsRoutine<Target>() where Target : MonoBehaviour
    {
        while (true)
        {
            Target[] activeTargetsList = FindObjectsOfType<Target>();

            foreach (Target target in activeTargetsList)
            {

                LaunchRocket(transform, target.transform);
            }

            yield return new WaitForSeconds(reloadTime);
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

        GameObject rocket = Instantiate(rocketPrefab, source.position + Vector3.up, lookRotation);
        rocket.GetComponent<RocketLogic>().SetTarget(target);
    }

    public void StopRocketAttack()
    {
        StopAllCoroutines();
    }

    #endregion
}
