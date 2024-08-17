using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private PlayerMovement _player;
    private Transform _playerTransform;
    private Vector3 _position;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        TrySetPlayerTransform();
        _position = transform.position;
    }

    private void TrySetPlayerTransform()
    {
        if (_player != null)
        {
            _playerTransform = _player.transform;
        }
        else
        {
            string errorMsg = $"{nameof(_player)} is null, can not move camera position. - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        UpdateVerticalPosition();
    }

    private void UpdateVerticalPosition()
    {
        _position.y = _playerTransform.position.y;
        transform.position = _position;
    }

    #endregion
}
