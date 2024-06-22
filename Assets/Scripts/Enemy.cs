using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy_default = 0,
    Enemy_powerful,
    Enemy_fast,
    Enemy_boss,
    Enemy_range,

    Enemy_group,

    Enemy_firstIndex = 0,
    Enemy_lastIndex = Enemy_group,
    Enemy_numberOfEnemy = Enemy_lastIndex + 1,
}

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters 
    // -----------------------------------------------------------------------

    #region Parameters

    [Header("Main Settings")]

    [SerializeField] EnemyType type;
    [SerializeField] float basicSpeed = 3f;
    [SerializeField] float pushForce = 1f;
    float pushForceMultiplier = 0.1f;

    Rigidbody rb;
    Vector3 direction = Vector3.zero;

    RocketLauncher rocketLauncher;

    [Space]
    [Header("Destroy bounds Settings")]

    [SerializeField] float defaultBound = -10f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rocketLauncher = GetComponentInChildren<RocketLauncher>();
    }

    private void Start()
    {
        if(rocketLauncher != null)
        {
            rocketLauncher.StartRocketAttack<PlayerController>();
        }
    }

    private void Update()
    {
        if(transform.position.y < defaultBound)
        {
            DestroyEnemy();
        }

        Transform playerTransform = FindObjectOfType<PlayerController>().transform;
        direction = (playerTransform.position - transform.position).normalized;
    }


    private void FixedUpdate()
    {
        rb.AddForce(direction * basicSpeed);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Push player
        
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(player != null)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            Vector3 direction = (player.transform.position - transform.position).normalized;
            float actualSpeed = playerRb.velocity.magnitude;

            playerRb.AddForce(direction * (pushForce * pushForceMultiplier) * actualSpeed, ForceMode.Impulse);
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void DestroyEnemy()
    {
        // TODO : Add score for player 
        Destroy(gameObject);
    }

    public EnemyType GetEnemyType()
    {
        if (type == EnemyType.Enemy_numberOfEnemy)
        {
            type = EnemyType.Enemy_lastIndex;
            Debug.Log($"# Warning -> {gameObject.name} - PowerUpType == Power_numberOfPowers. Return LAST power up.");
        }

        return type;
    }

    #endregion

}
