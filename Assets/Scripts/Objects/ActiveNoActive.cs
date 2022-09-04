using UnityEngine;

public class ActiveNoActive : MonoBehaviour
{
    public bool activeOnStart = true;
    public GameObject[] activeGameObject;

    private void Awake()
    {
        ActiveDesactive(activeOnStart);
    }
    public void ActiveDesactive(bool active)
    {
        for (int i = 0; i < activeGameObject.Length; i++)
        {
            activeGameObject[i].SetActive(active);
        }
    }

    public void ActiveDesactiveButton()
    {
        gameObject.SetActive(false);
    }
}
