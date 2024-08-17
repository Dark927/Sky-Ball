using UnityEngine;

public class SingletonBaseGlobal<Type> : MonoBehaviour where Type : SingletonBaseGlobal<Type>
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    public static Type Instance;

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected virtual void Awake()
    {
        SetInstance();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = (Type)this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
