using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public static GameScript Instance;

    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject victoryMenu;

    [HideInInspector]
    public bool isPaused = false;
    private CanvasGroup pauseTintCanvasGroup;
    private CanvasGroup deathTintCanvasGroup;
    private CanvasGroup victoryTintCanvasGroup;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        victoryMenu.SetActive(false);

        pauseTintCanvasGroup = pauseMenu.transform.Find("PauseTint").GetComponent<CanvasGroup>();
        deathTintCanvasGroup = deathMenu.transform.Find("DeathTint").GetComponent<CanvasGroup>();
        victoryTintCanvasGroup = victoryMenu.transform.Find("VictoryTint").GetComponent<CanvasGroup>();
        pauseTintCanvasGroup.alpha = 0f;
        deathTintCanvasGroup.alpha = 0f;
        victoryTintCanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
        pauseTintCanvasGroup.alpha = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
        pauseTintCanvasGroup.alpha = 0f;
    }

    public void ShowDeathMenu()
    {
        //Time.timeScale = 0f;
        //isPaused = true;
        deathMenu.SetActive(true);
        deathTintCanvasGroup.alpha = 1f;
    }

    public void ShowVictoryMenu()
    {
        //Time.timeScale = 0f;
        //isPaused = true;
        victoryMenu.SetActive(true);
        victoryTintCanvasGroup.alpha = 1f;
    }
}
