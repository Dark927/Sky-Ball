using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [Header("Main Settings")]
    [Space]

    [SerializeField] private float _basicSpeed = 5f;
    [SerializeField] private Transform _focalPoint;

    [Space]

    private bool _onGround = true;
    private Vector3 _groundContactPoint;
    private Rigidbody _playerRb;
    private float _forwardInput = 0;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public float BasicSpeed => _basicSpeed;
    public bool OnGround => _onGround;
    public Vector3 GroundContactPoint => _groundContactPoint;

    public void SetGroundContactInfo(Vector3 groundContactPoint)
    {
        _onGround = true;
        _groundContactPoint = groundContactPoint;
    }

    public void SetGroundContactInfo(bool onGround)
    {
        _onGround = onGround;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerRb.sleepThreshold = 0.0f;
    }

    private void Update()
    {
        if (transform.position.y < PositionManager.Instance.DeathBoundY) 
        {
            GameManager.Instance.RestartGame();
        }

        _forwardInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        _playerRb.AddForce(_focalPoint.forward * _basicSpeed * _forwardInput, ForceMode.Acceleration);
    }

    #endregion
}

