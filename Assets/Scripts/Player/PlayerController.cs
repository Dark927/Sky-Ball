using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [Header("Main Settings")]
    [Space]

    [SerializeField] private float _basicSpeed = 5f;
    [SerializeField] private Transform _focalPoint;
    [SerializeField] private float _pushForce = 2f;

    [Space]

    [SerializeField] private string _groundTag = "Ground";
    private bool _onGround = true;
    private Vector3 _groundContactPoint;

    private Rigidbody _playerRb;

    private float _forwardInput = 0;

    [SerializeField] private float _fallBoundY = -5f;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public bool OnGround => _onGround;
    public float PushForce => _pushForce;
    public Vector3 GroundContactPoint => _groundContactPoint;

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
        if (transform.position.y < _fallBoundY)
        {
            RestartGame();
        }

        _forwardInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        _playerRb.AddForce(_focalPoint.forward * _basicSpeed * _forwardInput);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_groundTag))
        {
            _onGround = true;
            _groundContactPoint = collision.GetContact(0).point;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(_groundTag))
        {
            _onGround = false;
        }
    }


    private void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    #endregion
}

