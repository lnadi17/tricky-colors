using UnityEngine;
using System.Collections;

public class PlatformTransform : MonoBehaviour {

	private GameObject cam = CameraScript.cam; //4use
	private float distanceBetween = Generator.distanceBetween; //4use
	private int timesY = Generator.timesY; 

	SpriteRenderer rdr;

	/*void OnBecameInvisible(){
		if (cam.transform.position.y > transform.position.y) {
			//Do things when object dissapears.
		}
	}*/

	void Start(){
		rdr = transform.GetChild (0).GetComponent<SpriteRenderer> (); //Usually transformManager is added last.

		StartCoroutine (Reposition());
	}

	IEnumerator Reposition(){
		while (true){
			if (!rdr.isVisible && cam.transform.position.y > transform.position.y){
				print ("Fired");
				transform.Translate(new Vector2(0, distanceBetween * timesY)); //5 is the number of lines generated. could be variable.
			}
			yield return new WaitForSeconds (1f);
		}
	}
}