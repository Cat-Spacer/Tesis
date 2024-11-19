using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;
    protected bool _onPause;
    void Start()
    {
        _menu.SetActive(false);
    }
    public virtual void OpenMenu()
    {
        Time.timeScale = 0;
        _menu.SetActive(true);
    }
    public virtual void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public virtual void Resume()                                 
    {                                             
        Time.timeScale = 1;                       
        _onPause = false;                         
        _menu.SetActive(false);              
    }
    public virtual void Pause()
    {
        Time.timeScale = 0;
        _onPause = true;
        _menu.SetActive(true);
    }
    public virtual void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public virtual void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public virtual void LevelSelector()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }
}
