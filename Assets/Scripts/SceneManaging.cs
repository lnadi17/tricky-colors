using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManaging: MonoBehaviour {

	public Text bestext;
	public GameObject canvasObject;

	private GameObject pauseObject;
	private GameObject resumeObject;

	private void Start(){
		if (PlayerPrefs.HasKey ("BestScore")) {
			bestext.text = "BEST: " + PlayerPrefs.GetInt ("BestScore");	
		} else {
			bestext.enabled = false;
		}
		pauseObject = canvasObject.transform.FindChild ("pauseBtn").gameObject;
		resumeObject = canvasObject.transform.FindChild ("resumeBtn").gameObject;
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
		pauseObject.SetActive (false);
		resumeObject.SetActive (true);
		Time.timeScale = 0;
	}
	public void ResumeGame(){
		pauseObject.SetActive (true);
		resumeObject.SetActive (false);
		Time.timeScale = 1;
	}
}
