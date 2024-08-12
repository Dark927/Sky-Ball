using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    private EnemyHead _head;
    private Rigidbody _rb;
    private Vector3 _startPosition;

    private List<EnemyBody> _childrenBodyList;


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public virtual void Die()
    {
        gameObject.SetActive(false);
        ResetSettings();

        foreach (EnemyBody childBody in _childrenBodyList)
        {
            childBody.gameObject.SetActive(true);
            childBody.ResetSettings();
        }

        _head.TryDeactivate();
    }

    public void ResetSettings()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(_head.transform.position + _startPosition, Quaternion.Euler(Vector3.zero));
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
        _startPosition = transform.position;
        _childrenBodyList = new List<EnemyBody>(GetComponentsInChildren<EnemyBody>().Where(child => child.gameObject != gameObject));
    }

    #endregion
}
