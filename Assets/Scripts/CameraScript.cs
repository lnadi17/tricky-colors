using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public static GameObject cam;
	public GameObject player;

	void Awake(){ //It's initialized in Awake 'coz otherwise PlayedDie's start funcion is fired first.
		cam = gameObject; //PlatformTransform refers GameObject component of camera from that.
	}

	void Update () {
		//Camera moves smoothly.
		if (cam.transform.position.y - player.transform.position.y < 3){
			Vector3 futurePos = player.transform.position + new Vector3 (0, 3, -10);
			cam.transform.position = Vector3.Lerp (cam.transform.position, futurePos, 0.05f);
		}
	}
}
