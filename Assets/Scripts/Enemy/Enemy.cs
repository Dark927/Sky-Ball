using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Enums
    // -----------------------------------------------------------------------

    #region Enums

    public enum TYPE
    {
        Default = 0,
        Powerful,
        Fast,
        Boss,
        Range,

        Group,

        FirstIndex = 0,
        LastIndex = Group,
        NumberOfEnemy = LastIndex + 1,
    }

    #endregion

    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Main Settings")]

    [SerializeField] private TYPE _type;
    [SerializeField] private float _basicSpeed = 3f;
    [SerializeField] private float _pushForce = 1f;
    private float _pushForceMultiplier = 0.1f;

    private Transform _playerTransform;
    private Rigidbody _rb;
    private Vector3 _moveDirection = Vector3.zero;

    [Space]
    [Header("Destroy bounds Settings")]

    [SerializeField] private float _deathBoundY = -10f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected virtual void Start()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y < _deathBoundY)
        {
            DestroyEnemy();
        }

        _moveDirection = (_playerTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_moveDirection * _basicSpeed);
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

            playerRb.AddForce(direction * (_pushForce * _pushForceMultiplier) * actualSpeed, ForceMode.Impulse);
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

        Destroy(transform.root.gameObject);
    }

    public TYPE GetEnemyType()
    {
        if (_type == TYPE.NumberOfEnemy)
        {
            _type = TYPE.LastIndex;
            Debug.Log($"# Warning -> {gameObject.name} - PowerUpType == Power_numberOfPowers. Return LAST power up.");
        }

        return _type;
    }

    #endregion
}
