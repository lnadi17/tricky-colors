using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	public List<GameObject> colorHolders;
	public GameObject colorHolder;
	public static float distanceBetween = 3f;
	//
	public static int timesX = 7;
	public static int timesY = 50;
	//
	public static bool doFlip = false;
	public Sprite mySprite;
	public static float speed1 = 0;
	public float speed;

	private List<GameObject> tempHolders;
	private int e = 0; //Ordinality

	void Start () {
		speed1 = speed;
		GenerateY (timesY);
	}
		
	void GenerateX(float positionOffsetY, int times = 1){

		//Changes the direction of every second platform.
		doFlip = false;
		if (e % 2 == 0){
			doFlip = true;
		}
		e++;

		float positionOffsetX = 0 - (colorHolders.Count * 1.5f * 0.5f * times) + 0.75f; //It's always in the middle of the screen.
		GameObject colorHolderInstance = Instantiate (colorHolder, new Vector2 (positionOffsetX, positionOffsetY), Quaternion.identity) as GameObject;
		colorHolderInstance.AddComponent<PlatformTransform> ();

		//Adds new GameObject in order to tell if line's gone out of sight.
		GameObject transformManager = new GameObject ("TransformManager");
		transformManager.transform.position = new Vector2 (0, positionOffsetY);
		transformManager.transform.SetParent (colorHolderInstance.transform);
		transformManager.transform.localScale = new Vector3 (colorHolders.Count * 1.5f * times, 0.35f, 1);

		SpriteRenderer renderer = transformManager.AddComponent<SpriteRenderer> ();
		renderer.sprite = mySprite;
		renderer.color = Color.clear;

		for (int u = 0; u < times; u++) {
			tempHolders = new List<GameObject> (colorHolders);
			for (int i = 0; i < colorHolders.Count; i++) {
				int randomIndex = Random.Range (0, tempHolders.Count);
				GameObject toInstantiate = tempHolders [randomIndex];
				tempHolders.RemoveAt (randomIndex);

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
