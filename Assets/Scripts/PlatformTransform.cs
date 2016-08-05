using UnityEngine;
using System.Collections;

public class PlatformTransform : MonoBehaviour {

	private GameObject cam; 
	private float distanceBetween = Generator.distanceBetween; 
	//private int timesX = Generator.timesX;
	private int timesY = Generator.timesY; 
	private GameObject tManager;
	private SpriteRenderer rdr;
	private float tManagerWidth;
	private float tManagerSafeZone;
	private float speed;
	private float cameraWidth; //Height is twice camera's (orthographic) size, so we only need to declare its width.
	private bool inRange = false;
	private bool dontAsk = false;

	void Awake(){
		speed = Generator.speed1;
		if(Generator.doFlip){
			speed *= -1;
		}
	}

	void Start(){
		cam = CameraScript.cam;
		RetriveCameraSize (); //That is really cool method tho.
		//It's better to move platforms using platformManager GameObject 'coz it's default coordinates are on zero.
		tManager = transform.GetChild (0).gameObject; //Usually transformManager is added last.
		rdr = tManager.GetComponent<SpriteRenderer> ();
		tManagerWidth = tManager.transform.localScale.x;
		tManagerSafeZone = 1.5f * 3; //When 3 tiles far from the edge of the screen.
		StartCoroutine (Reposition());
	}

	void RetriveCameraSize (){
		Camera cam1 = Camera.main; //The first enabled camera tagged "MainCamera".
		//cameraHeight = cam1.orthographicSize * 2; I don't need that.
		cameraWidth = (cam1.orthographicSize * 2)  * cam1.aspect; //Camera width is its height multiplied by camera's aspect (aspect is height/width).
	}


	IEnumerator Reposition(){
		while (true){
			if (!rdr.isVisible && cam.transform.position.y > transform.position.y){
				transform.Translate(new Vector2(0, distanceBetween * timesY)); //timesY is the number of lines generated.
			}
			yield return new WaitForSeconds (1f);
		}
	}

	void FixedUpdate(){

		//I can't think about any other solution even though it seems easy.
		//I'm not in the mood of thinking.
		if (!dontAsk) {
			if (Mathf.Abs (tManager.transform.position.x) <= tManagerWidth * 0.5f - cameraWidth * 0.5f - tManagerSafeZone) {
				inRange = true;
			} else {
				inRange = false;
			}
		}

		if (inRange) {
			transform.Translate (0.1f * speed, 0, 0);
			dontAsk = false;
		} else {
			speed *= -1;
			inRange = true;
			dontAsk = true;
		}
	}
}