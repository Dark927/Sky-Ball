using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RocketType
{
    Rocket_player = 0,
    Rocket_enemy,

    Rocket_firstIndex = 0,
    Rocket_lastIndex = Rocket_enemy,
    Rocket_numberOfRockets = Rocket_lastIndex + 1,
}

public class RocketLogic : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters 

    [Header("Main Settings")]
    [Space]

    Transform target;
    Vector3 rocketDirection;
    Vector3 explodeBound = new Vector3(0, -0.7f, 0);

    [SerializeField] RocketType rocketType = RocketType.Rocket_firstIndex;
    [SerializeField] float speed = 10f;
    [SerializeField] float rocketForce = 2f;
    [SerializeField] bool autoAiming = false;


    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        if (target != null)
        {
            CalculateAutoAim(target);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if ((target == null) || (transform.position.y < explodeBound.y))
        {
            Explode();
            return;
        }

        RocketFly(target);
    }

    private void RocketFly(Transform target)
    {
        if (autoAiming)
        {
            CalculateAutoAim(target);
        }

        transform.position += (rocketDirection * speed * Time.deltaTime);
    }

    private void CalculateAutoAim(Transform target)
    {
        rocketDirection = CalculateDirection(target);
        ConfigureRotation(target);
    }

    private Vector3 CalculateDirection(Transform target)
    {
        return (target.position - transform.position).normalized;
    }

    private void ConfigureRotation(Transform target)
    {
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider targetÑollider)
    {
        switch (rocketType)
        {
            case RocketType.Rocket_player:
                {
                    PushEnemyTarget(targetÑollider);
                }
                break;



            case RocketType.Rocket_enemy:
                {
                    PushPlayerTarget(targetÑollider);
                }
                break;
        }
    }

    private void PushPlayerTarget(Collider target)
    {
        PlayerController player = target.GetComponent<PlayerController>();

        if (player != null)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            Vector3 pushDirection = (player.transform.position - transform.position).normalized;

            playerRb.AddForce(pushDirection * rocketForce, ForceMode.Impulse);
            Explode();
        }
    }

    private void PushEnemyTarget(Collider target)
    {
        Enemy enemy = target.GetComponent<Enemy>();

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
