using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	public List<GameObject> colorHolders;
	public GameObject colorHolder;
	public static float distanceBetween = 3f;
	public static int timesX = 4;
	public static int timesY = 10;
	public Sprite mySprite;

	void Start () {
		GenerateY (timesY);
		print ("Generating finished.");
	}

	//Generates lines randomly, but it'd be great if it didn't create 2 rects next to each other with the same color.
	void GenerateX(float positionOffsetY, int times = 1){
		float positionOffsetX = 0 - (colorHolders.Count * 1.5f * 0.5f * times) + 0.75f; //It's always in the middle of the screen.
		GameObject colorHolderInstance = Instantiate(colorHolder, new Vector2(positionOffsetX, positionOffsetY), Quaternion.identity) as GameObject;
		colorHolderInstance.AddComponent<PlatformTransform> ();

		GameObject transformManager = new GameObject ("TransformManager");
		transformManager.transform.position = new Vector2 (0, positionOffsetY);
		transformManager.transform.SetParent (colorHolderInstance.transform);
		transformManager.transform.localScale = new Vector3 (colorHolders.Count * 1.5f * times, 1, 1);

		SpriteRenderer renderer = transformManager.AddComponent<SpriteRenderer> ();
		renderer.sprite = mySprite;
		renderer.color = Color.clear;

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
			GenerateX (positionOffsetY, timesX);
			positionOffsetY += distanceBetween;
		}
	}
}
