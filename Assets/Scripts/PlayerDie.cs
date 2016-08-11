using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDie : MonoBehaviour {

	private GameObject cam;
	private Rigidbody2D rb2d;
	private bool isOver = false;
	private PlayerMovement playerMovement;
	private CircleCollider2D circleCollider;
	private SpriteRenderer rdr;
	//public Color fadedColor; //Changes color when collides with different-colored GameObject.
	public GameObject canvas;
	public Text text;

	void Start () {
		playerMovement = GetComponent<PlayerMovement> ();
		circleCollider = GetComponent<CircleCollider2D> ();
		//rdr = GetComponent<SpriteRenderer> ();
		cam = CameraScript.cam;
		rb2d = GetComponent<Rigidbody2D> ();
		StartCoroutine (IfDead ());
	}
	
	IEnumerator IfDead(){
		while (true) {
			if(cam.transform.position.y - transform.position.y >= 5) // Camera's height in Unity units is always 10.
			{
				GameOver ();
			}
			yield return new WaitForSeconds (0.05f);
		}
	}

	void GameOver(){
		playerMovement.enabled = false; //Just disable player's controls.
		circleCollider.enabled = false; //Not to interact with other objects.
		rb2d.isKinematic = true;
		isOver = true;
		if (ScoreScript.myScore > PlayerPrefs.GetInt ("BestScore")) {
			PlayerPrefs.SetInt ("BestScore", ScoreScript.myScore);
		}
	}

	void FixedUpdate(){
		if(isOver){
			if (transform.localScale.y < 30) { //Scale with the value '30' is big enough to cover all screens.
				transform.localScale *= 1.05f;
			} else {
				canvas.SetActive (true);
				text.text = "SCORE: " + ScoreScript.myScore;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (!other.CompareTag (gameObject.tag) && other.tag != "Untagged") {
			playerMovement.enabled = false;
			tag = "Untagged";
			//rdr.color = fadedColor;
		}
	}
}
