
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{ 
    
    public GameObject Popups,PausePopup,GamePlay,NextPopup,GameOverPopup;
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Debug.LogWarning("More than one Tile Manager");
    }
    
    public void Exit()=> Application.Quit();

    public void PlayButton()
    {
        Popups.SetActive(true);
        GamePlay.SetActive(false);
        PausePopup.SetActive(true);
    }
    public void CloseButton()
    {   GamePlay.SetActive(true);
        Popups.SetActive(false);
        PausePopup.SetActive(false);
    }
    public void NextLevel()
    {   Popups.SetActive(true);
        GamePlay.SetActive(false);
        NextPopup.SetActive(true);
    }
    public void GameOver()
    {
        Popups.SetActive(true);
        GamePlay.SetActive(false);
        GameOverPopup.SetActive(true);
    }

}
