using UnityEngine;

public class StaticScreen : MonoBehaviour, IDelayer
{

    private void Start()
    {
        SetToManager();
        if(gameObject.activeSelf) gameObject.SetActive(false);
    }

    public void SetToManager(GameManager parent = default)
    {
        //if (!parent && GameManager.instance) GameManager.instance.StaticScreens(this);
    }
}
