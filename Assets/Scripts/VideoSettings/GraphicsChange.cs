using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GraphicsChange : MonoBehaviour
{
    [SerializeField] private Toggle _fullscreen = default, _vsync = default;
    [SerializeField] private int _resIndex = 0, _aaIndex = 0;
    [SerializeField] private TMP_Dropdown _qualityDropdown = default, _resolutionDropdown = default;

    private Resolution[] _resolutions = default;

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

        _resolutions = Screen.resolutions;
        foreach (var res in _resolutions)
        {
            Debug.Log(res.width + "x" + res.height + " : " + res.refreshRateRatio);
        }

        #region Resolution Dropdown

        _resolutionDropdown = GetComponentInChildren<TMP_Dropdown>();
        _resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            options.Add($"{_resolutions[i].width} x {_resolutions[i].height}");
            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)                
                _resIndex = i;
        }

        #if UNITY_EDITOR
        _resIndex = 1;
        #endif

        options.Reverse();
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = _resIndex;
        _resolutionDropdown.RefreshShownValue();
        #endregion
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