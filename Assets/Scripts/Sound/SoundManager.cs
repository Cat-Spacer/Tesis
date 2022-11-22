using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager instance;
    float _baseVolume;

    public Dictionary<string,float> mixerValue = new Dictionary<string, float>();
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
    }
    private void Update()
    {
        /*if (FindObjectOfType<GameManager>())
        {
            Play(Types.MusicForest);
            Play(Types.WoodAmbientSound);
            Play(Types.Rain);
        }*/
        //if (Input.GetKeyDown(KeyCode.P)) //Probar sonido
        //    SoundManager.instance.Play(SoundManager.Types.VineCrunch);
    }
    public void Play(Types name,bool loop = true)
    {
        Sound s = Array.Find(sounds, sound => sound.nameType == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.loop = loop;
        s.source.Play();
    }
    public void Pause(Types name)
    {
        Sound s = Array.Find(sounds, sound => sound.nameType == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
        // StartCoroutine(FadeOut(s.source, 0.05f));


        //s.source.Pause();
    }
    public void PauseAll()
    {
        foreach (var s in sounds)
            s.source.Pause();
    }
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            Debug.Log("A");
                yield return null;
        }

        audioSource.Pause();
        audioSource.volume = startVolume;
    }

    
    public enum Types
    {
        WindForest,
        Steps,
        ForestAmbience,
        MusicForest,
        CatDamage,
        CatAttack,
        CatDash,
        CatJump,
        Dash,
        Item,
        Climb,
        FallingDebris,
        WoodAmbientSound,
        FlowerWind,
        Rain,
        CarnivorousPlant,
        PlayerDeath,
        Mushroom,
        VineCrunch,
        MagicCat,
        ClestialDiamond
    }
    public void OnClickSound()
    {
       // instance.Play(Types.Click);
    }
}
