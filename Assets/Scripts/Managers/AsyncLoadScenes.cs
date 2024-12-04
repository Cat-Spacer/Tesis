using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AsyncLoadScenes : MonoBehaviour
{
    public static int sceneToLoad = 3;
    [SerializeField] private int _sceneToLoad = 0;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _textPercentage;

    private void Awake()
    {
        _sceneToLoad = sceneToLoad;
        if (_sceneToLoad < 1) _sceneToLoad = 1;
        if (!_progressBar) _progressBar = FindObjectOfType<Slider>();
        if (!_textPercentage) _textPercentage = _progressBar.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (_sceneToLoad >= 0 && _sceneToLoad < SceneManager.sceneCountInBuildSettings && _progressBar)
            ChargeAsyncScene(_sceneToLoad);
    }

    public void ChargeAsyncScene(int scene)
    {
        var async = SceneManager.LoadSceneAsync(scene);
        Application.backgroundLoadingPriority = ThreadPriority.High;//High si hay escena de carga Low si se carga dentro de la escena.
        StartCoroutine(ChargeCoroutine(async));
    }

    private IEnumerator ChargeCoroutine(AsyncOperation async)
    {
        while (!async.isDone)
        {
            if (_progressBar && _textPercentage)
            {
                _progressBar.value = async.progress;
                _textPercentage.text = Mathf.Round(async.progress * 100.0f).ToString() + "%";
            }
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;
    }
}