using TMPro;
using UnityEngine;

public class WinMenu : MenuButtons
{
    [SerializeField] TextMeshProUGUI p1Text;
    [SerializeField] TextMeshProUGUI p2Text;
    public void OpenMenu(string p1Txt, string p2Txt)
    {
        p1Text.text = p1Txt;
        p2Text.text = p2Txt;
        _menu.SetActive(true);
    }
}
