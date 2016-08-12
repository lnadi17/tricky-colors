using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorScript : MonoBehaviour {

	public List<Color> colorList; //RGBYP

	private SpriteRenderer rdr;

	void Start () {
		rdr = GetComponent<SpriteRenderer> ();
	}
	
	void Update () {
		
		switch (gameObject.tag) {
		case "Red":
			rdr.color = colorList [0];
			break;
		case "Green":
			rdr.color = colorList [1];
			break;
		case "Blue":
			rdr.color = colorList [2];
			break;
		case "Yellow":
			rdr.color = colorList [3];
			break;
		case "Purple":
			rdr.color = colorList [4];
			break;
		default:
			break;
		}

	}
}
