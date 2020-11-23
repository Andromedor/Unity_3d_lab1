using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected ServiceManager _serviceManager;
    protected UIAudioManager _audioManager;

    [SerializeField] protected GameObject _menu;
   


    [Header("HeaderButton")]
    [SerializeField] protected Button _play;
    [SerializeField] protected Button _settings;
    [SerializeField] protected Button _quit;
   
    [Header("Settings")]
    [SerializeField] protected GameObject _settingsMenu;
    [SerializeField] protected Button _closeSettings;
    protected virtual  void Start()
    {
        _serviceManager = ServiceManager.Instance;
        _audioManager = UIAudioManager.Instance;
        _quit.onClick.AddListener(OnQuuitClicked);
        _settings.onClick.AddListener(OnSettingsCliked);
        _closeSettings.onClick.AddListener(OnSettingsCliked);
    }
    protected virtual void OnDestroy()
    {
       
        _quit.onClick.RemoveListener(OnQuuitClicked);
        _settings.onClick.RemoveListener(OnSettingsCliked);
        _closeSettings.onClick.RemoveListener(OnSettingsCliked);
    }
    protected virtual void OnMenuCliked()
    {
        _menu.SetActive(!_menu.activeInHierarchy);
       
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
              OnMenuCliked();
    }
   
    private void OnQuuitClicked()
    {
        _audioManager.Play(UiClipNames.Quit);
        _serviceManager.Quit();
        
    }
    private void OnSettingsCliked()
    {
        _audioManager.Play(UiClipNames.Settings);
        _settingsMenu.SetActive(!_settingsMenu.activeInHierarchy);
    }
}
