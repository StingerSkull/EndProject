using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pauseBtn;
    public GameObject player;
    public AudioSource audioSource;

    public TextMeshProUGUI textTime; 

    private bool isPaused = false;
    private bool canPause = true;
    private float chrono;

    public int Score { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(isPaused);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        chrono = 0;
        textTime.text = "0";
    }

    private void Update()
    {
        if (canPause)
        {
            chrono += Time.deltaTime;
        }
        float min = Mathf.FloorToInt(chrono / 60);
        float sec = Mathf.FloorToInt(chrono % 60);
        float milli = (chrono - (min * 60 + sec)) * 1000;
        textTime.text = string.Format("{0:00}:{1:00}:{2:000}", min, sec,milli); ;
    }

    public void Pause()
    {
        if (player.activeSelf && canPause)
        {
            isPaused = !isPaused;
            pausePanel.SetActive(isPaused);
            
            if (isPaused)
            {
                Time.timeScale = 0f;
                audioSource.Pause();
            }
            else
            {
                Time.timeScale = 1f;
                audioSource.Play();
            }
        }
    }

    public void Win()
    {
        canPause = false;
        winPanel.SetActive(true);
        pauseBtn.SetActive(false);
        Time.timeScale = 0f;
        audioSource.Pause();
    }
    public void Lose()
    {
        canPause = false;
        losePanel.SetActive(true);
        pauseBtn.SetActive(false);
        Time.timeScale = 0f;
        audioSource.Pause();
    }

    public void ReloadScene()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeScene);
        Time.timeScale = 1f;
    }
}
