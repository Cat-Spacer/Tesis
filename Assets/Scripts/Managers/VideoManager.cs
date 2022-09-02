using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VideoPlayer))]
public class VideoManager : MonoBehaviour
{
    VideoPlayer videoPlayer
    {
        get { return GetComponent<VideoPlayer>(); }
    }
    public int mainMenuScene = 1;
    public float duration = 5f;
    private void Start()
    {
        StartCoroutine(WaitForEndOfVideo());
    }
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0))
            SceneManager.LoadScene(mainMenuScene);
    }

    IEnumerator WaitForEndOfVideo()
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(mainMenuScene);
    }
}
