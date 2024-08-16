using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private string _groundTag = "Ground";
    [SerializeField] private float _pushForce = 2f;

    private PlayerMovement _playerMovement;
    private Rigidbody _rigidbody;

    public float PushForce => _pushForce;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_groundTag))
        {
            _playerMovement.SetGroundContactInfo(collision.GetContact(0).point);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(_groundTag))
        {
            _playerMovement.SetGroundContactInfo(false);
        }
    }

    #endregion
}
