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
        return (collision.gameObject.GetComponent<Enemy>() != null);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Spawn(GameObject enemyToSpawn)
    {
        Vector3 offsetY = new Vector3(0, enemyToSpawn.transform.position.y, 0);
        Instantiate(enemyToSpawn, transform.position + offsetY, Quaternion.identity);
    }

    public bool IsBlocked()
    {
        return _isBlocked;
    }

    #endregion 

}
