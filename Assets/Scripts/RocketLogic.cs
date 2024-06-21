using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLogic : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters 

    [Header("Main Settings")]
    [Space]

    Transform target;
    [SerializeField] float speed = 10f;
    [SerializeField] float rocketForce = 2f;

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    // Update is called once per frame
    private void Update()
    {
        if (target == null)
        {
            Explode();
            return;
        }

        MoveToTarget(target);
    }

    private void MoveToTarget(Transform target)
    {
        Vector3 rocketDirection = (target.position - transform.position).normalized;

        transform.position += (rocketDirection * speed * Time.deltaTime);
        transform.LookAt(target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            Vector3 pushDirection = (enemy.transform.position - transform.position).normalized;

            enemyRb.AddForce(pushDirection * rocketForce, ForceMode.Impulse);
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
    }


    #endregion

    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    #endregion
}
