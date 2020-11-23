using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseGameMenuController
{
    [Header("Menu")]
    [SerializeField] private Button _chooseLvl;
    [SerializeField] private Button _reset;
   

    [SerializeField] private GameObject _lvlMenu;
    [SerializeField] private Button _closeLvl;
    int _lvl = 1;
    protected override void Start()
    {
        base.Start();
        _chooseLvl.onClick.AddListener(OnLvlMenuCliced);
        _closeLvl.onClick.AddListener(OnLvlMenuCliced);
      //  PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey(GamePrefs.LastPlayerLvl.ToString()))
        {
            _play.GetComponentInChildren<TMP_Text>().text = "Resume";
            _lvl = PlayerPrefs.GetInt(GamePrefs.LastPlayerLvl.ToString());
        }
        _play.onClick.AddListener(OnPlayClicked);
        _reset.onClick.AddListener(OnResetClicer);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _chooseLvl.onClick.RemoveListener(OnLvlMenuCliced);
        _closeLvl.onClick.RemoveListener(OnLvlMenuCliced);
        //  PlayerPrefs.DeleteAll();
       
        _play.onClick.RemoveListener(OnPlayClicked);
        _reset.onClick.RemoveListener(_serviceManager.ResetProgres);
    }
    private void OnLvlMenuCliced()
    {
        _lvlMenu.SetActive(!_lvlMenu.activeInHierarchy);
        OnMenuCliked();
        _audioManager.Play(UiClipNames.ChooseLvl);
    }
    private void OnPlayClicked()
    {
        _serviceManager.ChangeLvl(_lvl);
        _audioManager.Play(UiClipNames.Play);
    }
    private void OnResetClicer()
    {
        _play.GetComponentInChildren<TMP_Text>().text = "Play";

        _serviceManager.ResetProgres();
    }
}
