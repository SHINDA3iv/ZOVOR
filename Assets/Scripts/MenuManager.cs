using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject Menu;


    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    
    public void LoadGame()
    {
        Menu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadLeaderboard()
    {
        Menu.SetActive(false);
    }

    public void ExitLeaderboard()
    {
        Menu.SetActive(true);
    }
}
