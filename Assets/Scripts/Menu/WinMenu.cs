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
            animator.enabled = false;
            _menu.SetActive(true);
        }
        p1Text.text = catPoints.ToString();
        p2Text.text = hamsterPoints.ToString();
        
    }
    public void StopAnimation()
    {
        animator.enabled = false;
        catWinMenu.SetActive(false);
        hamsterWinMenu.SetActive(false);
    }
}
