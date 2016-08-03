using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public static GameObject cam;

	void Start () {
		cam = gameObject; //PlatformTransform refers GameObject component of camera from that.
	}
	
	void Update () {
	
	}
}
