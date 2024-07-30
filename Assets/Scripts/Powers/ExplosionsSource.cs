using UnityEngine;

public class ExplosionsSource : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Explosions Settings")]
    [Space]

    private ObjectsPool _explosionsPool;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _explosionsPool = GetComponent<ObjectsPool>();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void ActivateExplosion(Vector3 position, float force)
    {
        GameObject explosionObject = _explosionsPool.RequestInactiveObject(true);
        explosionObject.transform.position = position;
        explosionObject.SetActive(true);

        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.Explode(force);
    }

    #endregion
}
