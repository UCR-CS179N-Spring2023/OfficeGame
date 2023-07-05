using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PlayGame()
    {
        audioManager.PlaySound();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelManager");
    }

    public void QuitGame()
    {
        audioManager.PlaySound();
        Debug.Log("Quit");
        Application.Quit();
    }
}
