using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    // --------------
    // Inspector 
    // --------------

    #region Inspector Fields

    [Header("Main Settings")]

    [SerializeField] private float _basicSpeed = 3f;

    [Space]

    #endregion

    // --------------

    private EnemyBody _body;
    private Rigidbody _rb;
    private Transform _playerTransform;
    private Vector3 _moveDirection = Vector3.zero;

    public float BasicSpeed => _basicSpeed;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _body = GetComponentInParent<EnemyBody>();
    }

    private void Start()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            string errorMsg = $"Can not find the player in the scene. - {gameObject.name}, player is null.";
            ErrorsManager.Instance.SendErrorMsg(errorMsg);
        }
    }

    private void Update()
    {
        if (IsOutOfBounds())
        {
            _body.Die();
        }

        CalculateMoveDirection();
    }


    private void FixedUpdate()
    {
        Move();
    }

    private bool IsOutOfBounds() => (transform.position.y < PositionManager.Instance.DeathBoundY);

    private void CalculateMoveDirection()
    {
        _moveDirection = (_playerTransform.position - transform.position).normalized;
    }

    private void Move()
    {
        _rb.AddForce(_moveDirection * _basicSpeed, ForceMode.Acceleration);
    }

    #endregion
}
