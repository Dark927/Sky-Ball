using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    private EnemyHead _head;
    private Rigidbody _rb;


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Die()
    {
        gameObject.SetActive(false);
        ResetAllForces();

        _head.Deactivate();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _head = GetComponentInParent<EnemyHead>();
        _rb = GetComponent<Rigidbody>();
    }

    private void ResetAllForces()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    #endregion
}
