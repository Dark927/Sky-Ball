using System.Collections;
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

    [SerializeField] private float _boostForce = 10f;
    [SerializeField] private float _boostActiveTime = 0.25f;
    [SerializeField] private float _boostStoppingTime = 0.75f;
    private Coroutine _activeBoost = null;

    private bool _onGround = true;
    private Vector3 _groundContactPoint;
    private Rigidbody _rigidbody;
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
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = 0.0f;
    }

    private void Update()
    {
        CheckDeathBound();

        _forwardInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && (_activeBoost == null))
        {
            _activeBoost = StartCoroutine(BoostRoutine());
        }
    }

    private void CheckDeathBound()
    {
        if (transform.position.y < PositionManager.Instance.DeathBoundY)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_focalPoint.forward * (_basicSpeed * _forwardInput), ForceMode.Acceleration);
    }

    private IEnumerator BoostRoutine()
    {
        int forwardDirection = _forwardInput > 0 ? 1 : -1;

        Vector3 startVelocity = _focalPoint.forward * _rigidbody.velocity.magnitude * forwardDirection;
        _rigidbody.velocity = startVelocity;
        _rigidbody.AddForce(_focalPoint.forward * (_boostForce * _forwardInput), ForceMode.Impulse);

        float elapsedTime = 0f;

        yield return new WaitForSeconds(_boostActiveTime);

        Vector3 boostedVelocity = _rigidbody.velocity;

        while (elapsedTime < _boostStoppingTime)
        {
            elapsedTime += Time.deltaTime;
            _rigidbody.velocity = Vector3.Lerp(boostedVelocity, startVelocity, elapsedTime / _boostStoppingTime);
            yield return null;
        }

        _activeBoost = null;
    }

    #endregion
}

