using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GraphicsChange : MonoBehaviour
{
    [SerializeField] private Toggle _fullscreen = default, _vsync = default;
    [SerializeField] private int _resIndex = 0;
    [SerializeField] private TMP_Dropdown _qualityDropdown = default, _resolutionDropdown = default;

    private List<Resolution> _resolutions = new ();

    private void Awake()
    {
        
        Setting();
    }

    private void Setting()
    {
        if (QualitySettings.vSyncCount == 0)
            _vsync.isOn = false;
        else
            _vsync.isOn = true;


        _fullscreen.isOn = Screen.fullScreen;
        
        _resolutionDropdown.ClearOptions();
        List<string> options = new ();

        _resIndex = 0;
        
        _resolutions.Add(new Resolution { width = 1920, height = 1080 });
        _resolutions.Add(new Resolution { width = 1366, height = 768 });
        _resolutions.Add(new Resolution { width = 1280, height = 720 });
        _resolutions.Add(new Resolution { width = 1024, height = 576 });
        
        // for (int i = 0; i < _resolutions.Count; i++)
        // {
        //     if (Mathf.RoundToInt(_resolutions[i].refreshRateRatio.numerator) == 60 && (_resolutions[i].width % 16 == 0 && _resolutions[i].height % 9 == 0))
        //     {
        //         string option = $"{_resolutions[i].width} x {_resolutions[i].height}";
        //         if (!options.Contains(option))
        //         {
        //             options.Add(option);
        //             if (_resolutions[i].width == Screen.currentResolution.width &&
        //                 _resolutions[i].height == Screen.currentResolution.height)
        //                 _resIndex = i;
        //         }
        //     }
        // }
        
        foreach (Resolution res in _resolutions)
        {
            string option =  $"{res.width} x {res.height}";
            options.Add(option);
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = 0;
        _resolutionDropdown.RefreshShownValue();
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullscreen.isOn);
    }

    public void SetResolutionValue()
    {
        _resIndex = _resolutionDropdown.value;
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = _fullscreen.isOn;

        if (_vsync.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        SetResolution(_resIndex);
    }
}