using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Vector3 endDistance;
	[Range(1, 10)]public float transpeed;
	public AudioClip myAudio;
	public GameObject particleObj;
	//public ParticleSystem pSystem;

	private Rigidbody2D rb2d;
	private bool started;
	private CircleCollider2D coll;
	private Animator anim;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		coll = GetComponent<CircleCollider2D> ();
		anim = GetComponent<Animator> ();
		transform.localScale = new Vector2 (25, 25);
		coll.enabled = false;
		started = false;
		StartCoroutine (Shrink ());
	}

	void Update () {
		//if (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.y < Camera.main.pixelHeight * 0.8f && started){
		if ((Input.GetKeyDown(KeyCode.Space) && started) || (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.y < Camera.main.pixelHeight * 0.8f && started)){
			if(rb2d.bodyType == RigidbodyType2D.Kinematic){
				rb2d.bodyType = RigidbodyType2D.Dynamic;
			}
			Vector3 end = endDistance + (Vector3)rb2d.position;
			StartCoroutine (SmoothMovement (end));
			AudioSource.PlayClipAtPoint (myAudio, Camera.main.transform.position - new Vector3(0, -3f, 0));
			anim.Play ("PlayerAnim", -1, 0f);
		}
	}

	IEnumerator Shrink(){
		while (started == false){
			transform.localScale = new Vector2 (transform.localScale.x - 0.5f, transform.localScale.y - 0.5f);
			if (transform.localScale.x <= 0.5f) {
				transform.localScale = new Vector2 (0.5f, 0.5f);
				coll.enabled = true;
				started = true;
			}
			yield return new WaitForSeconds (.025f);
		}
	}

	IEnumerator SmoothMovement (Vector3 end){
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while(sqrRemainingDistance > 0.1f){
			Vector3 nextPosition = Vector3.MoveTowards (rb2d.position, end, transpeed * Time.deltaTime);
			transform.position = nextPosition;
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			rb2d.velocity = Vector3.zero;
			yield return null;
		}
	}
}
