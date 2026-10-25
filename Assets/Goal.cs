
using UnityEngine;

public class Goal : MonoBehaviour
{
    private bool completed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed || !other.CompareTag("Ball"))
            return;

        completed = true;

        Debug.Log("Goal reached!");

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CompleteLevelScore();
            Debug.Log("Score added successfully!");
        }
        else
        {
            Debug.LogError("ScoreManager Instance is NULL!");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteLevel();
        }
        else
        {
            Debug.LogError("GameManager is missing!");
        }
    }
}
