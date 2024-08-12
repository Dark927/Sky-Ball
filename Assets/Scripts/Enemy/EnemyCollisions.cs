using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class EnemyCollisions : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    // --------------
    // Inspector 
    // --------------

    #region Inspector Fields

    [Header("Settings")]

    [SerializeField] private float _pushForce = 1f;
    [SerializeField] private float _speedDiffMultiplierMax = 1.5f;
    [SerializeField] private float _speedDiffMultiplierMin = 0.5f;

    #endregion

    // --------------

    private Rigidbody _rb;


    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Push player
        
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            Push(player);
        }
    }

    private void Push(PlayerMovement player)
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();


        float ownActualSpeed = _rb.velocity.magnitude;
        float playerActualSpeed = playerRb.velocity.magnitude;
        float speedDiffMultiplier = (ownActualSpeed > playerActualSpeed) ? _speedDiffMultiplierMax : _speedDiffMultiplierMin;

        Vector3 pushDirection = (player.transform.position - transform.position).normalized;

        playerRb.AddForce(speedDiffMultiplier * _pushForce * (pushDirection), ForceMode.Impulse);
    }

    #endregion
}
