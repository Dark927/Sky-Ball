using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ErrorsManager : MonoBehaviour
{
    public static ErrorsManager Instance;

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
        SetInstance();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CheckManagersState();
    }

    private void CheckManagersState()
    {
        bool isFatal = false;

        List<string> managersNames = new List<string>()
        {
          nameof(DifficultyManager),
          nameof(EnemySpawner),
          nameof(PositionManager),
          nameof(PowersSpawner),
          nameof(WavesManager),
        };

        List<bool> managersCreated = new List<bool>()
        {
          DifficultyManager.Instance != null,
          EnemySpawner.Instance!= null,
          PositionManager.Instance!= null,
          PowersSpawner.Instance != null,
          WavesManager.Instance != null,
        };


        for (int currentManager = 0; currentManager < managersCreated.Count; currentManager++)
        {
            if (!managersCreated[currentManager])
            {
                SendErrorMsg($"{managersNames[currentManager]} is null !");
                isFatal = true;
            }
        }

        if (isFatal)
        {

            SendErrorMsg("Some of Managers is null, can not start the game.", true);
        }
    }

    #endregion

}
