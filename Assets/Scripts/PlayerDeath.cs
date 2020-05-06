using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public ParticleSystem deathEmitter;
    public Gradient deathGradient;
    public GameObject gameManager;
    public float gameOverPanelDelay;
    public GameObject finalScoreObject;

    private Text finalScoreText;
    private PlayerMovement playerMovement;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private SceneController sceneController;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sceneController = gameManager.GetComponent<SceneController>();
        finalScoreText = finalScoreObject.GetComponent<Text>();

        StartCoroutine(CheckDead());
    }

    IEnumerator CheckDead() {
        while (true) {
            if (Camera.main.transform.position.y - transform.position.y >= 5) {
                // Tweak for better particle explosion.
                transform.Translate(0, 1f, 0);
                transform.localScale = new Vector3(0.2f, 1.7f, 1);
                // Call GameOver function after that.
                GameOver();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void GameOver() {
        playerMovement.enabled = false;
        circleCollider.enabled = false;
        // Update best score after death.
        if (ScoreController.score > PlayerPrefs.GetInt("BestScore")) {
            PlayerPrefs.SetInt("BestScore", ScoreController.score);
        }
        // Launch particles, disable sprite renderer and stop camera.
        UpdateGradient();
        deathEmitter.Emit(100);
        spriteRenderer.enabled = false;
        Camera.main.GetComponent<CameraScript>().enabled = false;
        // Wait and then show game over panel.
        sceneController.Invoke("GameOverAction", gameOverPanelDelay);
        // Update final score text.
        finalScoreText.text = "SCORE: " + ScoreController.score;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!enabled) return;
        if (!CompareTag(other.tag) && !other.CompareTag("Score")) {
            // In this case player and obstacle colors don't match.
            GameOver();
        }
    }

    void UpdateGradient() {
        var col = deathEmitter.colorOverLifetime;
        col.enabled = true;

        // Change only first and last keys in gradient color keys.
        GradientColorKey[] newGradientColorKeys = new GradientColorKey[deathGradient.colorKeys.Length];
        
        for (int i = 0; i < deathGradient.colorKeys.Length; i++) {
            if (i == 0 || i == deathGradient.colorKeys.Length - 1) {
                newGradientColorKeys[i] = new GradientColorKey(spriteRenderer.color, 0f);
            } else {
                newGradientColorKeys[i] = deathGradient.colorKeys[i];
            }
        }

        deathGradient.SetKeys(newGradientColorKeys, deathGradient.alphaKeys);
        col.color = deathGradient;
    }
}
