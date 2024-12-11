using UnityEngine;

public class LoseMenu : MenuButtons
{ 
    //[SerializeField] Animation anim;
    protected override void OnFinishGame(object[] obj)
    {
        _onFinishGame = true;
        //menu.SetActive(true);
        //anim.Play();
    }
}
