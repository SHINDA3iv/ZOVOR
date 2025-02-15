using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject menuCanvas;

    void Update()
    {
        CheckIfOutOfBounds();
    }

    public void StopGame()
    {
        if (teleportTarget != null)
            transform.position = teleportTarget.position;
        menuCanvas.SetActive(true);
    }

    void CheckIfOutOfBounds()
    {
        if (transform.position.y < -1)
        {
            Invoke("StopGame", 2.0f);
        }
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
