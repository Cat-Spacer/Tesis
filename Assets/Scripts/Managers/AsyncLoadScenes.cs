using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AsyncLoadScenes : MonoBehaviour
{
    public static int sceneToLoad = 1;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private int _sceneToLoad = 0;
    [SerializeField] private TextMeshProUGUI _textPercentage;

    private void Awake()
    {
        _sceneToLoad = sceneToLoad;
        if (_progressBar != false) _progressBar = FindObjectOfType<Slider>();
    }

    private void Start()
    {
        if (_sceneToLoad >= 0 && _sceneToLoad < SceneManager.sceneCountInBuildSettings && _progressBar != null)
            ChargeAsyncScene(_sceneToLoad);
    }

    public void ChargeAsyncScene(int scene)
    {
        var async = SceneManager.LoadSceneAsync(scene);

        StartCoroutine(ChargeCoroutine(async));
    }

    private IEnumerator ChargeCoroutine(AsyncOperation async)
    {
        while (!async.isDone)
        {
            if (_progressBar != null && _textPercentage != null)
            {
                _progressBar.value = async.progress;
                //Debug.Log($"Async progress = {async.progress}");
                _textPercentage.text = Mathf.Round(async.progress * 100.0f).ToString() + "%";
            }
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;
    }
}
