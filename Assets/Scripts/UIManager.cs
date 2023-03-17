
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject Popups,PausePopup,GamePlay;
    public GameObject GameOverPanel,Next;
    
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
    public void NextLevel()=>SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    public void Restart()=> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void Exit()=> Application.Quit();

    public void PlayButton()
    {
        Popups.SetActive(true);
        PausePopup.SetActive(true);
        
    } 

    public void CloseButton()
    {
        GamePlay.SetActive(true);
        Popups.SetActive(false);
        print("close close");
    } 

}
