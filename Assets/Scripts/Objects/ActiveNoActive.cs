using UnityEngine;

public class ActiveNoActive : MonoBehaviour
{
    public GameObject[] activeGameObject;
    public GameObject[] desactiveGameObject;

    private void Awake()
    {
        ActiveDesactive();
    }
    public void ActiveDesactive()
    {
        for (int i = 0; i < activeGameObject.Length; i++)
            if (desactiveGameObject[i]) activeGameObject[i].SetActive(true);
        for (int j = 0; j < desactiveGameObject.Length; j++)
            if(desactiveGameObject[j]) desactiveGameObject[j].SetActive(false);
    }

    public void ActiveDesactiveButton()
    {
        gameObject.SetActive(false);
    }
}
