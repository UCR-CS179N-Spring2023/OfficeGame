using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour
{
    public Button nextButton;
    public Button menuButton;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        nextButton.onClick.AddListener(NextGame);
        menuButton.onClick.AddListener(OpenMenu);
    }

    public void NextGame()
    {
        audioManager.PlaySound();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenMenu()
    {
        audioManager.PlaySound();
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelManager");
    }
}
