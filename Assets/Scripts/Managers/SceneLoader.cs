using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonBaseGlobal<SceneLoader>
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    private ASyncLoader _asyncLoader;
    [SerializeField][HideInInspector] private string _gameplayScenePath;
    [SerializeField][HideInInspector] private string _mainMenuScenePath;

    [SerializeField] private float _startDelay = 0.5f;
    [SerializeField] private GameObject _loadingScreenUI;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

#if UNITY_EDITOR

    public SceneAsset GameplayScene
    {
        get
        {
            if (!string.IsNullOrEmpty(_gameplayScenePath))
            {
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(_gameplayScenePath);
            }
            return null;
        }

        set
        {
            _gameplayScenePath = AssetDatabase.GetAssetPath(value);
        }
    }

    public SceneAsset MainMenuScene
    {
        get
        {
            if (!string.IsNullOrEmpty(_mainMenuScenePath))
            {
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(_mainMenuScenePath);
            }
            return null;
        }

        set
        {
            _mainMenuScenePath = AssetDatabase.GetAssetPath(value);
        }
    }

#endif

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadGameplayScene()
    {
        LoadSceneByPath(_gameplayScenePath);
    }

    public void LoadMainMenuScene()
    {
        LoadSceneByPath(_mainMenuScenePath);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();

        _asyncLoader = GetComponent<ASyncLoader>();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void LoadSceneByPath(string scenePath)
    {
        string sceneName = ConvertScenePathToName(scenePath);
        _asyncLoader.LoadSceneASync(sceneName, _loadingScreenUI, _startDelay);
    }

    private string ConvertScenePathToName(string path)
    {
        int nameStartIndex = path.LastIndexOf('/') + 1;
        int nameEndIndex = path.LastIndexOf('.');

        if ((0 <= nameStartIndex) && (nameStartIndex < nameEndIndex))
        {
            return path[nameStartIndex..nameEndIndex];
        }
        else
        {
            string errorMsg = $"Scene path ({path}) is not correct, can not find the scene name. - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg, true);
            return null;
        }
    }

    #endregion
}
