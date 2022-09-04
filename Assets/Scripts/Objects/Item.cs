using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    [SerializeField] private int _sceneToLoad = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
