using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManaging: MonoBehaviour {

	public Text bestext;
	public GameObject canvasObject;
	public GameObject player;
	public Sprite AudioOnSprite;
	public Sprite AudioOffSprite;

	private GameObject pauseObject;
	private GameObject resumeObject;
	private GameObject exitObject;
	private GameObject soundObject;
	private Image currentAudioImage;
	private PlayerMovement pScript;

	private void Start(){
		if (PlayerPrefs.HasKey ("BestScore")) {
			bestext.text = "BEST: " + PlayerPrefs.GetInt ("BestScore");	
		} else {
			bestext.enabled = false;
		}
		pauseObject = canvasObject.transform.FindChild ("pauseBtn").gameObject;
		resumeObject = canvasObject.transform.FindChild ("resumeBtn").gameObject;
		soundObject = canvasObject.transform.FindChild ("volBtn").gameObject;
		exitObject = canvasObject.transform.Find ("exitBtn").gameObject;
		currentAudioImage = soundObject.GetComponent<Image> ();
		pScript = player.GetComponent<PlayerMovement> ();
		if (AudioListener.volume == 1) {
			currentAudioImage.sprite = AudioOnSprite;
		} else {
			currentAudioImage.sprite = AudioOffSprite;
		}
	}

	private void Update(){
		//When player presses Android's back button.
		if (SceneManager.GetActiveScene().name == "Menu" && Input.GetKey(KeyCode.Escape)){ 
			LeaveGame ();
		}
	}

	public void TurnAudio(){
		if (AudioListener.volume == 1) {
			currentAudioImage.sprite = AudioOffSprite;
			AudioListener.volume = 0;
		} else {
			currentAudioImage.sprite = AudioOnSprite;
			AudioListener.volume = 1;
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
		ResumeGame ();
	}
	public void LeaveGame(){
		Application.Quit ();
	}
	public void PauseGame(){
		pScript.enabled = false;
		pauseObject.SetActive (false);
		resumeObject.SetActive (true);
		soundObject.SetActive (true);
		exitObject.SetActive (true);
		Time.timeScale = 0;
	}
	public void ResumeGame(){
		pScript.enabled = true;
		pauseObject.SetActive (true);
		resumeObject.SetActive (false);
		soundObject.SetActive (false);
		exitObject.SetActive (false);
		Time.timeScale = 1;
	}
}
