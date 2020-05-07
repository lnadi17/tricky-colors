using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class ScoreController : MonoBehaviour
{
    public static int score; // Player score. it's accessible from other places too.

    public ParticleSystem scoreEmitter;
    public GameObject player;
    public GameObject scoreTextObject;
    public AudioClip myAudio;
    public float distanceBetweenY;

    private TextMesh scoreText;
    private SpriteRenderer scoreSpriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    private ColorScript colorScript;

    void Start() {
        score = 0;

        scoreText = scoreTextObject.GetComponent<TextMesh>();
        scoreText.text = score.ToString();

        scoreSpriteRenderer = GetComponent<SpriteRenderer>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        colorScript = GetComponent<ColorScript>();

        int colorIndex = GetRandomColorIndex();
        player.tag = GetColorTag(colorIndex);
        playerSpriteRenderer.color = colorScript.colorList[colorIndex];
        scoreText.color = playerSpriteRenderer.color;
    }

    string GetColorTag(int index) {
        return "Color " + index.ToString();
    }

    int GetRandomColorIndex() {
        return Random.Range(0, colorScript.colorList.Count);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Change player tag and color.
        int colorIndex = GetRandomColorIndex();
        other.tag = GetColorTag(colorIndex);
        playerSpriteRenderer.color = colorScript.colorList[colorIndex];
        // Play score pickup audio.
        AudioSource.PlayClipAtPoint(myAudio, Camera.main.transform.position);
        // Increase score count and update text.
        score++;
        scoreText.text = score.ToString();
        scoreText.color = playerSpriteRenderer.color;
        scoreEmitter.Emit(7);
        scoreTextObject.transform.Translate(0, distanceBetweenY, 0);
        transform.localPosition += new Vector3(0, distanceBetweenY, 0);
    }
}
