using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ErrorsManager : MonoBehaviour
{
    public static ErrorsManager instance;

    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void SendWarningMsg(string message)
    {
        Debug.LogWarning("# Warning :: " + message);
    }

    public void SendErrorMsg(string message, bool isFatal = false)
    {
        if (!isFatal)
        {
            Debug.LogError("# Error :: " + message);
        }
        else
        {
            Debug.LogError("# Fatal Error :: " + message);

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif

            Application.Quit();
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

}
