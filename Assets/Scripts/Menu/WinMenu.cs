using TMPro;
using UnityEngine;

public class WinMenu : MenuButtons
{
    [SerializeField] TextMeshProUGUI p1Text;
    [SerializeField] TextMeshProUGUI p2Text;
    [SerializeField] private GameObject tvMenu;
    [SerializeField] Animator animator;
    [SerializeField] private GameObject catWinMenu;
    [SerializeField] private GameObject hamsterWinMenu;
    public void OpenMenu(int catPoints, int hamsterPoints)
    {
        if (catPoints > hamsterPoints)
        {
            animator.Play("WinMenuAnim");
        }
        else if (hamsterPoints > catPoints)
        {
            animator.Play("HamsterWinAnim");
        }
        else
        {
            Debug.Log("Empate");
            _menu.SetActive(true);
            animator.Play("WinMenu");
        }
        p1Text.text = catPoints.ToString();
        p2Text.text = hamsterPoints.ToString();
        
        foreach (var button in tv.tvButtons) button.gameObject.SetActive(false);
        tv.restartBtn.gameObject.SetActive(true);
        tv.nextLevelBtn.gameObject.SetActive(true);
        tv.levelsBtn.gameObject.SetActive(true);
        tv.optionsBtn.gameObject.SetActive(true);
        tv.menuBtn.gameObject.SetActive(true);
    }
    public void StopAnimation()
    {
        //animator.enabled = false;
        catWinMenu.SetActive(false);
        hamsterWinMenu.SetActive(false);
        //animator.Play("WinMenu");
    }
}
