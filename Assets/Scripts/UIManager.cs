using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject Pause;
    
    public void NextLevel()=>SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    public void Restart()=> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void Exit()=> Application.Quit();

    public void PauseButton() => Pause.SetActive(true);





}
