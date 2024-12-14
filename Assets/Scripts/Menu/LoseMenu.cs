using System.Collections;
using UnityEngine;

public class LoseMenu : MenuButtons
{ 
    [SerializeField] Animator anim;
    protected override void Start()
    {
        base.Start();
        anim.enabled = false;
    }

    protected override void OnFinishGame(object[] obj)
    {
        EventManager.Instance.Trigger(EventType.ShowTv);
        _onFinishGame = true;
        StartCoroutine(OnFinishGame());
    }

    IEnumerator OnFinishGame()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance) GameManager.Instance.EnableByBehaviour();
        _menu.SetActive(true);
        anim.enabled = true;
    }
}
