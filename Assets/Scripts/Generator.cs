using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	public List<GameObject> colorHolders;
	public GameObject colorHolder;
	public float distanceBetween;

	void Start () {
		CameraScript.cam.transform.position = new Vector3 (0, 0, -10); //Remove this line and see if it works when level restarts.
		GenerateY (5);
		print ("Generating finished.");
	}

	//Generates lines randomly, but it'd be great if it didn't create 2 rects next to each other with the same color.
	void GenerateX(float positionOffsetY, int times = 1){
		float positionOffsetX = 0 - (colorHolders.Count * 1.5f * 0.5f * times) + 0.75f; //It's always in the middle of the screen.
		GameObject colorHolderInstance = Instantiate(colorHolder, new Vector2(positionOffsetX, positionOffsetY), Quaternion.identity) as GameObject;
		colorHolderInstance.AddComponent<PlatformTransform> ();
		colorHolderInstance.AddComponent<SpriteRenderer> ().sprite = new Sprite ();
		for (int u = 0; u < times; u++) {
			for (int i = 0; i < colorHolders.Count; i++) {
				int randomIndex = Random.Range (0, colorHolders.Count);
				GameObject toInstantiate = colorHolders [randomIndex];
				GameObject instance = Instantiate (toInstantiate, new Vector2 (positionOffsetX, positionOffsetY), Quaternion.identity) as GameObject;
				instance.transform.SetParent (colorHolderInstance.transform);
				positionOffsetX += 1.5f;
			}
		}
	}

	//Generates lines on Y axis. Default is 5 times.
	void GenerateY(int times = 5){
		float positionOffsetY = 0;
		for (int i = 0; i < times; i++) {
			GenerateX (positionOffsetY, 2);
			positionOffsetY += distanceBetween;
		}
	}
}
