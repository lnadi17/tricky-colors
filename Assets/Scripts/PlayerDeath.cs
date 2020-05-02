using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private CircleCollider2D circleCollider;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        circleCollider = GetComponent<CircleCollider2D>();

        StartCoroutine(CheckDead());
    }

    IEnumerator CheckDead() {
        while (true) {
            if (Camera.main.transform.position.y - transform.position.y >= 5) {
                GameOver();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void GameOver() {
        playerMovement.enabled = false;
        // Update best score after death.
        if (ScoreController.score > PlayerPrefs.GetInt("BestScore")) {
            PlayerPrefs.SetInt("BestScore", ScoreController.score);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!enabled) return;
        Debug.Log("Trigger Enter");
        if (!CompareTag(other.tag) && !other.CompareTag("Score")) {
            // In this case player and obstacle colors don't match.
            GameOver();
        }
    }
}
