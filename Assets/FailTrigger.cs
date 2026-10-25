using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FailLevel();
        }
        else
        {
            Debug.LogWarning("GameManager is missing!");
        }
    }
}