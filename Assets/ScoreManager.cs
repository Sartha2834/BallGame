
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Settings")]
    public int totalScore = 0;
    public int scorePerLevel = 100;

    [Header("UI Settings")]
    public TMP_Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    // Add points when the ball hits a target
    public void AddScore(int points)
    {
        totalScore += points;
        UpdateScoreUI();

        Debug.Log("Score updated: " + totalScore);
    }

    // Add points when a level is completed
    public void CompleteLevelScore()
    {
        AddScore(scorePerLevel);
    }

    // Update score text on Canvas
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
        else
        {
            Debug.LogWarning(
                "Score Text is not assigned in ScoreManager!"
            );
        }
    }
}
