using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private Slider _volume;
    [SerializeField] private Toggle _fullScreen;
    [SerializeField] TMP_Dropdown _resolutiondDropDown;
    [SerializeField] TMP_Dropdown _quality;
    private Resolution[] _availbleResolution;
    [SerializeField] TMP_Dropdown _qulityDropDown;
    private string[] _qualityLevels;

    [SerializeField] private AudioMixer _masterMixer;
    // Start is called before the first frame update
    void Start()
    {
        _volume.onValueChanged.AddListener(OnVolumeChanged);
        _fullScreen.onValueChanged.AddListener(OnFullScreenChanged);
        _availbleResolution = Screen.resolutions;
        _resolutiondDropDown.onValueChanged.AddListener(OnResolutionChanged);
        _resolutiondDropDown.ClearOptions();
        _qulityDropDown.onValueChanged.AddListener(OnQulityChanged);
        int currentIndex =0;
        List<string> options = new List<string>();
        for (int i = 0; i < _availbleResolution.Length; i++)
        {
            if (_availbleResolution[i].width <= 800)
                continue;
            options.Add(_availbleResolution[i].width + "x" + _availbleResolution[i].height);
            if (_availbleResolution[i].width == Screen.currentResolution.width && _availbleResolution[i].height == Screen.currentResolution.height)
                currentIndex = 1;
        }
        _resolutiondDropDown.AddOptions(options);
        _resolutiondDropDown.value = currentIndex;
        _resolutiondDropDown.RefreshShownValue();
        _qualityLevels = QualitySettings.names;
        _qulityDropDown.ClearOptions();
        _qulityDropDown.AddOptions(_qualityLevels.ToList());
        int qualityLvl = QualitySettings.GetQualityLevel();
        _qulityDropDown.value = qualityLvl;
        _qulityDropDown.RefreshShownValue();
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        _volume.onValueChanged.RemoveListener(OnVolumeChanged);
        _fullScreen.onValueChanged.RemoveListener(OnFullScreenChanged);
        _resolutiondDropDown.onValueChanged.RemoveListener(OnResolutionChanged);
        _qulityDropDown.onValueChanged.RemoveListener(OnQulityChanged);

    }
    private void OnVolumeChanged(float volume)
    {
        _masterMixer.SetFloat("Volume", volume);
    }
    private void OnFullScreenChanged(bool value)
    {
        Screen.fullScreen = value;
    }

    private void OnResolutionChanged(int resolutionIndex)
    {
        Resolution resolution = _availbleResolution[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void OnQulityChanged(int qualityLvl)
    {
        QualitySettings.SetQualityLevel(qualityLvl, true);
    }
}
