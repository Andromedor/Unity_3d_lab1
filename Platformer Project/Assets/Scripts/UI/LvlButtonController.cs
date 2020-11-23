using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvlButtonController : MonoBehaviour
{
    private Button _button;
    private TMP_Text _lvl;
    private ServiceManager _serviceManager;
    [SerializeField] private Scenes scenes;
    void Start()
    {
        _button = GetComponent<Button>();
        if (!PlayerPrefs.HasKey(GamePrefs.LvlPlayer.ToString() + ((int)scenes).ToString()))
        {
            _button.interactable = false;
            return;
        }
            
        _button.onClick.AddListener(OnChangeLvlClicked);
        GetComponentInChildren<TMP_Text>().text = ((int)scenes).ToString();
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnChangeLvlClicked()
    {
        ServiceManager.Instance.ChangeLvl((int)scenes);
    }
}
