using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ScoreScript : MonoBehaviour {

	public GameObject player;
	public GameObject scoreTextGO;
	public List<string> tagList;
	public AudioClip myAudio;
	public static int myScore; //Player score.

	private float distanceBetween;
	private TextMesh scoreText;

	void Start(){
		myScore = 0;
		scoreText = scoreTextGO.GetComponent<TextMesh> ();
		scoreText.text = myScore.ToString();
		distanceBetween = Generator.distanceBetween;
		player.tag = RandomColor ();
	}

	string RandomColor(){
		string tag = tagList [Random.Range (0, tagList.Count)];
		return tag;
	}

	void OnTriggerEnter2D(Collider2D other){
		AudioSource.PlayClipAtPoint (myAudio, Camera.main.transform.position);
		myScore++;
		scoreText.text = myScore.ToString ();
		scoreTextGO.transform.Translate (0, distanceBetween, 0);
		other.tag = RandomColor ();
		transform.position = new Vector2 (transform.position.x, transform.position.y + distanceBetween);
	}
}
