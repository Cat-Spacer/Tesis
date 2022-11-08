using UnityEngine;

public class ActiveNoActive : MonoBehaviour
{
    //public bool activeOnStart = true;
    public GameObject[] activeGameObject;
    public GameObject[] desactiveGameObject;

    private void Awake()
    {
        ActiveDesactive();
    }
    public void ActiveDesactive()
    {
        for (int i = 0; i < activeGameObject.Length; i++)
            activeGameObject[i].SetActive(true);
        for (int j = 0; j < desactiveGameObject.Length; j++)
            desactiveGameObject[j].SetActive(false);
    }

    public void ActiveDesactiveButton()
    {
        gameObject.SetActive(false);
    }
}
