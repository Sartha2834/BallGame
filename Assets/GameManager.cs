
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject successPanel;
    public GameObject failPanel;

    [Header("Level Settings")]
    public string level2SceneName = "Level2";

    private bool levelCompleted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        levelCompleted = false;

        if (successPanel != null)
            successPanel.SetActive(false);

        if (failPanel != null)
            failPanel.SetActive(false);
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        if (successPanel != null)
            successPanel.SetActive(true);

        // Stop the game after Level 2 is completed.
        if (SceneManager.GetActiveScene().name == level2SceneName)
        {
            Time.timeScale = 0f;
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level1")
        {
            SceneManager.LoadScene(level2SceneName);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void FailLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        if (failPanel != null)
            failPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}
