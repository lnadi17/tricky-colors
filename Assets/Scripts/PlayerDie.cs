using UnityEngine;
using System.Collections;

public class PlayerDie : MonoBehaviour {

	private GameObject cam;
	private Rigidbody2D rb2d;
	bool isOver = false;

	void Start () {
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
			yield return new WaitForSeconds (0.05f); //I can't tell if it's faster than Update function though.
		}
	}

	void GameOver(){
		GetComponent<PlayerMovement> ().enabled = false; //Just disable player's controls.
		GetComponent<CircleCollider2D>().enabled = false; //Not to interact with other objects.
		isOver = true;
		rb2d.isKinematic = true;
	}

	void FixedUpdate(){
		if(isOver){
			if(transform.localScale.y < 30){ //Scale with the value '30' is big enough to cover all screens.
				transform.localScale *= 1.05f;
			}
		}
	}
}
