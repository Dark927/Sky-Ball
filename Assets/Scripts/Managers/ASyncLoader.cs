using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ASyncLoader : MonoBehaviour
{
    private Slider _progressSlider;
    private TextMeshProUGUI _loadingText;

    public void LoadSceneASync(string sceneName, GameObject loadingScreenUI)
    {
        _progressSlider = loadingScreenUI.GetComponentInChildren<Slider>();
        _loadingText = loadingScreenUI.GetComponentInChildren<TextMeshProUGUI>();

        StartCoroutine(LoadSceneASyncRoutine(sceneName, loadingScreenUI));
    }

    private IEnumerator LoadSceneASyncRoutine(string sceneName, GameObject loadingScreenUI)
    {
        loadingScreenUI.SetActive(true);

        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);

        if (_progressSlider != null)
        {
            const float loadMaxProgress = 0.9f;
            float loadProgress = 0f;

            while (!sceneLoadOperation.isDone)
            {
                loadProgress = Mathf.Clamp01(sceneLoadOperation.progress / loadMaxProgress);
                _progressSlider.value = loadProgress;
                yield return null;
            }
            _progressSlider.value = 0f;
        }

        loadingScreenUI.SetActive(false);
    }

}
