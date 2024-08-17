using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private float _rotationSpeed = 1f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    // Update is called once per frame

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, _rotationSpeed * horizontalInput * Time.deltaTime);
    }

    #endregion

}
