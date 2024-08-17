using UnityEngine;

public abstract class SingletonBase<Type> : MonoBehaviour where Type : SingletonBase<Type>
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
