using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameMenuController : BaseGameMenuController
{
   
    
    [SerializeField] private Button _restart;
    [SerializeField] private Button _backToMenu;
    

    // Start is called before the first frame update
  protected override  void Start()
    {
        base.Start();
        _restart.onClick.AddListener(_serviceManager.Restart);
        _backToMenu.onClick.AddListener(OnMainMenuClicked);
        _play.onClick.AddListener(OnMenuCliked);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _restart.onClick.RemoveListener(_serviceManager.Restart);
        _backToMenu.onClick.RemoveListener(OnMainMenuClicked);
        _play.onClick.RemoveListener(OnMenuCliked);

    }
    protected override void OnMenuCliked()
    {
        _audioManager.Play(UiClipNames.Play);
        base.OnMenuCliked();
        Time.timeScale = _menu.activeInHierarchy ? 0 : 1;
    }
   
    public void OnMainMenuClicked()
    {
        ServiceManager.Instance.ChangeLvl((int)Scenes.MainMenu);
    }
   
}
