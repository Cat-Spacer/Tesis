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
        
        foreach (var button in tv.tvButtons) button.gameObject.SetActive(false);
        tv.restartBtn.gameObject.SetActive(true);
        tv.levelsBtn.gameObject.SetActive(true);
        tv.optionsBtn.gameObject.SetActive(true);
        tv.menuBtn.gameObject.SetActive(true);
        
        _onFinishGame = true;
        StartCoroutine(OnFinishGame(type));
    }

    IEnumerator OnFinishGame(bool type)
    {
        yield return new WaitForSeconds(1.5f);
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
