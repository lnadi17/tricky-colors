using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManaging: MonoBehaviour {

	public Text bestext;

	private void Start(){
		if (PlayerPrefs.HasKey ("BestScore")) {
			bestext.text = "BEST: " + PlayerPrefs.GetInt ("BestScore");	
		} else {
			bestext.enabled = false;
		}
	}

	public void ReplayGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	public void StartGame(){
		SceneManager.LoadScene ("Main");
	}
	public void MainMenu(){
		SceneManager.LoadScene ("Menu");
	}
	public void LeaveGame(){
		Application.Quit ();
	}
	public void PauseGame(){
		Time.timeScale = 0;
	}
	public void ResumeGame(){
		Time.timeScale = 1;
	}
}
