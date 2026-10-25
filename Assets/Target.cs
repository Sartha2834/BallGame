
using UnityEngine;

public class Target : MonoBehaviour
{
    public int points = 10;

    private bool hit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hit || !other.CompareTag("Ball"))
            return;

        hit = true;

        Debug.Log("Target hit!");

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(points);
        }
        else
        {
            Debug.LogError("ScoreManager Instance is missing!");
        }

        Destroy(gameObject);
    }
}
