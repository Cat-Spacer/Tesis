using System.Collections;
using UnityEngine;

public class LoseMenu : MenuButtons
{ 
    [SerializeField] Animator anim;
    [SerializeField] private GameObject loseGameByTimeMenu;
    [SerializeField] private GameObject loseGamePunchMenu;
    private bool type;
    protected override void Start()
    {
        base.Start();
        loseGameByTimeMenu.SetActive(false);
        loseGamePunchMenu.SetActive(false);
    }

    protected override void OnLoseGame(object[] obj)
    {
        if (obj.Length - 1 >= 0)
        {
            type = (bool) obj[0];
        }
        EventManager.Instance.Trigger(EventType.ShowTv);
        _onFinishGame = true;
        StartCoroutine(OnFinishGame(type));
    }

    IEnumerator OnFinishGame(bool type)
    {
        yield return new WaitForSeconds(1.5f);
        if (GameManager.Instance) GameManager.Instance.EnableByBehaviour();
        anim.enabled = true;
        if (type)
        {
            anim.Play("LoseByTime");
        }
        else
        {
            anim.Play("AngryFace");
        }
    }
}
