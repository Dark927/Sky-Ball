using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    private Button _btn;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public Button Btn => _btn;

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected virtual void SetEventListeners()
    {
        // Base class does not implement this method.
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods


    private void Awake()
    {
        TrySetButtonReference();
    }

    private void TrySetButtonReference()
    {
        if (!TryGetComponent(out _btn))
        {
            string warningMsg = $"{nameof(_btn)} is null, disabling component. - {gameObject.name}";
            ErrorsManager.Instance.SendWarningMsg(warningMsg);
            enabled = false;
        }
    }

    private void Start()
    {
        SetEventListeners();
    }

    #endregion
}
