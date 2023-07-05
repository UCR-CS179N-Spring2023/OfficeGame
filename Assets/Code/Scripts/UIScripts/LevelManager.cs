using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PlayLevel1()
    {
        audioManager.PlaySound();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level 1");
    }

    public void PlayLevel2()
    {
        audioManager.PlaySound();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level 2");
    }
}
