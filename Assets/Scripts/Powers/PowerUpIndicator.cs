using UnityEngine;

public class PowerUpIndicator : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields
    
    [SerializeField] private Vector3 _indicatorOffset = new(0, -0.55f, 0);
    [SerializeField] private Vector3 _indicatorRotation = new(0, 90f, 0);
    private Material _indicatorMaterial;

    private Transform _playerTransform;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods
    
    public void SetColor(Color color)
    {
        _indicatorMaterial.color = color;
    }

    public void SetPlayerTransform(Transform player)
    {
        _playerTransform = player;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _indicatorMaterial = GetComponent<MeshRenderer>().material;
    }

    private void LateUpdate()
    {
        FollowParent();
        Rotate();
    }

    private void FollowParent()
    {
        transform.position = _playerTransform.position + _indicatorOffset;
    }

    private void Rotate()
    {
        transform.Rotate(_indicatorRotation * Time.deltaTime);
    }

    #endregion
}
