using UnityEngine;

public class PauseManager : SingletonBase<PauseManager>
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fieds

    private bool _paused = false;
    
    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public bool Paused => _paused;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Pause()
    {
        Time.timeScale = 0;
        _paused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        _paused = false;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods


    #endregion
}
