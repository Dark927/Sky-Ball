using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    bool isBlocked = false;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void OnTriggerEnter(Collider other)
    {
        if (IsEnemy(other))
        {
            isBlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsEnemy(other))
        {
            isBlocked = false;
        }
    }

    private bool IsEnemy(Collider collision)
    {
        return (collision.gameObject.GetComponent<Enemy>() != null);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Spawn(GameObject enemyToSpawn)
    {
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
    }

    public bool IsBlocked()
    {
        return isBlocked;
    }

    #endregion 

}
