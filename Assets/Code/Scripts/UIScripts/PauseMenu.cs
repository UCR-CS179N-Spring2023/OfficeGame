using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button menuButton;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        resumeButton.onClick.AddListener(ResumeGame);
        menuButton.onClick.AddListener(OpenMenu);
    }

    public void ResumeGame()
    {
        audioManager.PlaySound();
        Time.timeScale = 1;
        gameObject.SetActive(false);
        GameScript.Instance.isPaused = false; // Update GameScript.cs that the game is no longer paused
    }

    public void OpenMenu()
    {
        audioManager.PlaySound();
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelManager");
    }
}
