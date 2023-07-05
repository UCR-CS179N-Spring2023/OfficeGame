using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public Button restartButton;
    public Button menuButton;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(OpenMenu);
    }

    public void RestartGame()
    {
        audioManager.PlaySound();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void OpenMenu()
    {
        audioManager.PlaySound();
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelManager");
    }
}
