using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceManager : MonoBehaviour
{
    public static ServiceManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayerPrefs.SetInt(GamePrefs.LastPlayerLvl.ToString(), SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt(GamePrefs.LvlPlayer.ToString() + SceneManager.GetActiveScene().buildIndex,1);
        }
    }
    public void Restart()
    {
        ChangeLvl(SceneManager.GetActiveScene().buildIndex);
    }
    public void EndLevel()
    {
        ChangeLvl(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void ChangeLvl(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }
    public void ResetProgres()
    {
        PlayerPrefs.DeleteAll();
    }
     
}
public enum Scenes
{
    MainMenu,
    First,
    Second,
    Third,
}
public enum GamePrefs
{
    LastPlayerLvl,
    LvlPlayer,
}