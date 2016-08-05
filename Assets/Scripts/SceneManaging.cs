using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManaging: MonoBehaviour {
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
}
