using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MenuButtons
{
    [SerializeField] TextMeshProUGUI _catText;
    [SerializeField] TextMeshProUGUI _hamsterText;
    [SerializeField] private GameObject tvMenu;
    [SerializeField] Animator animator;
    [SerializeField] private GameObject catWinMenu;
    [SerializeField] private GameObject hamsterWinMenu;
    [SerializeField] private ParticleSystem sumScoreHamster;
    [SerializeField] private ParticleSystem sumScoreCat;
    [SerializeField] private Image face;

    public void OpenMenu(int catPoints, int hamsterPoints)
    {
        if (catPoints > hamsterPoints)
        {
            animator.Play("CatWinMenuAnim");
            StartCoroutine(ShowPoints(_catText, catPoints, sumScoreCat));
            StartCoroutine(ShowPoints(_hamsterText, hamsterPoints, sumScoreHamster));
        }
        else if (hamsterPoints > catPoints)
        {
            animator.Play("HamsterWinAnim");
            StartCoroutine(ShowPoints(_catText, catPoints, sumScoreCat));
            StartCoroutine(ShowPoints(_hamsterText, hamsterPoints, sumScoreHamster));
        }
        else if (hamsterPoints == catPoints && hamsterPoints>0)
        {
            _catText.text = catPoints.ToString();
            _hamsterText.text = hamsterPoints.ToString();
            _menu.SetActive(true);
            animator.Play("WinMenuHacked");
        }
        else
        {
            _catText.text = catPoints.ToString();
            _hamsterText.text = hamsterPoints.ToString();
            _menu.SetActive(true);
            animator.Play("WinMenu");
        }

      


        /* for (int i = 0; i <= catPoints; i++)
         {
             _catText.text = i.ToString();
             sumScoreCat.Play();
             if (_catWon && !_hamsterWon)
                 animator.Play("WinMenuAnim");
         }
         for (int i = 0; i <= hamsterPoints; i++)
         {
             _hamsterText.text = i.ToString();
             sumScoreHamster.Play();
             if (!_catWon && _hamsterWon)
                 animator.Play("HamsterWinAnim");
         }*/
        foreach (var button in tv.tvButtons) button.gameObject.SetActive(false);
        tv.restartBtn.gameObject.SetActive(true);
        tv.nextLevelBtn.gameObject.SetActive(true);
        tv.levelsBtn.gameObject.SetActive(true);
        tv.optionsBtn.gameObject.SetActive(true);
        tv.menuBtn.gameObject.SetActive(true);
    }

    private IEnumerator ShowPoints(TextMeshProUGUI textElement, int maxPoints, ParticleSystem particleSystem)
    {
        textElement.text = "0";
        yield return new WaitForSeconds(1f);
        for (int i = 1; i <= maxPoints; i++)
        {
            particleSystem.Play();
            textElement.text = i.ToString();
            SoundManager.instance.Play(SoundsTypes.Error);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void StopAnimation()
    {
        //animator.enabled = false;
        catWinMenu.SetActive(false);
        hamsterWinMenu.SetActive(false);
        //animator.Play("WinMenu");
    }

    void PlayHackSound()
    {
        if(SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.Hacking, gameObject);
    }

    void StopHackSound()
    {
        if(SoundManager.instance != null) SoundManager.instance.Pause(SoundsTypes.Hacking, gameObject);
    }

    void PlayPublicFeedbackAnimation()
    {
        face.sprite = PeaceSystem.instance.GetCurrentFace().sprite;
        int faceLevel = PeaceSystem.instance.GetCurrentFaceLevel();
        if (faceLevel < 0)
        {
            animator.Play("PublicFeedback_Sad");
        }
        else if (faceLevel == 0)
        {
            animator.Play("PublicFeedback_Neutral");
        }
        else
        {
            animator.Play("PublicFeedback_Happy");
        }
    }

    void PlayHappySound()
    {
        if(SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.CrowdHappy);
    }
    void PlaySadSound()
    {
        if(SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.CrowdSurprised);
    }
    void PlayNeutralSound()
    {
        if(SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.CrowdMeh);
    }
}
