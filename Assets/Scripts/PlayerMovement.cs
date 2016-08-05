using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Vector3 endDistance;
	[Range(1, 10)]public float transpeed;
	private Rigidbody2D rb2d;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetTouch(0).phase == TouchPhase.Began){
			if(rb2d.isKinematic){
				rb2d.isKinematic = false;
			}
			Vector3 end = endDistance + (Vector3)rb2d.position;
			StartCoroutine (SmoothMovement (end));
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
