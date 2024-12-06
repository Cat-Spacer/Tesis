using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Security.Cryptography;


public class GraphicsChange : MonoBehaviour
{
    [SerializeField] private Toggle _fullscreen = default, _vsync = default;
    [SerializeField] private int _resIndex = 0, _aaIndex = 0;
    [SerializeField] private TMP_Dropdown _qualityDropdown = default, _resolutionDropdown = default;

    Resolution[] _resolutions = default;

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

        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        if (_qualityDropdown) _qualityDropdown.value = 6;
    }

    public void ApplyGrapshics()
    {
        Screen.fullScreen = _fullscreen.isOn;

        if (_vsync.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        SetResolution(_resIndex);
        SetAntiAliasing(_aaIndex);
    }
}