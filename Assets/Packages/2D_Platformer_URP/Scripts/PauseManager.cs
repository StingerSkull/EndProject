using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool isPaused;
    [SerializeField] GameObject pausePanel;

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(); }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void PauseGame()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        
    }
}
