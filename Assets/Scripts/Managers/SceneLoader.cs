using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    public static SceneLoader Instance;

    private ASyncLoader _asyncLoader;
    [SerializeField][HideInInspector] private string _gameplayScenePath;

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

#endif


    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadGameplayScene()
    {
        string sceneName = ConvertScenePathToName(_gameplayScenePath);
        _asyncLoader.LoadSceneASync(sceneName, _loadingScreenUI);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        SetInstance();

        _asyncLoader = GetComponent<ASyncLoader>();
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
