using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    private bool _isBlocked = false;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public bool IsBlocked => _isBlocked;

    public void Spawn(EnemyHead enemyToSpawn)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, 0, transform.position.z);

        enemyToSpawn.transform.position = spawnPos;
        enemyToSpawn.gameObject.SetActive(true);
    }

    public void PrepareToSpawn()
    {
        _isBlocked = true;
    }

    public void ResetState()
    {
        _isBlocked = false;
    }

    #endregion 


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void OnTriggerEnter(Collider other)
    {
        if (IsEnemy(other))
        {
            _isBlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsEnemy(other))
        {
            _isBlocked = false;
        }
    }

    private bool IsEnemy(Collider collision)
    {
        return (collision.GetComponent<EnemyHead>() != null);
    }

    #endregion

}
