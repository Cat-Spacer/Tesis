using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI p1Text;
    [SerializeField] TextMeshProUGUI p2Text;

    public void SetText(string p1Txt, string p2Txt)
    {
        p1Text.text = p1Txt;
        p2Text.text = p2Txt;
    }
}
