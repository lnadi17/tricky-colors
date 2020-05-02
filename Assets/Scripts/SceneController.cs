using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Transform mainSceneCanvas;
    public Sprite audioOnSprite;
    public Sprite audioOffSprite;
    public Text bestText;

    private GameObject pauseButton;
    private GameObject resumeButton;
    private GameObject volumeButton;
    private GameObject exitButton;
    private GameObject surePanel;
    private Image currentAudioImage;

    private void Start() {
        // Update best score text. 
        if (PlayerPrefs.HasKey("BestScore")) {
            bestText.text = "BEST: " + PlayerPrefs.GetInt("BestScore");
        } else {
            bestText.enabled = false;
        }

        if (mainSceneCanvas != null) {
            pauseButton = mainSceneCanvas.Find("PauseButton").gameObject;
            resumeButton = mainSceneCanvas.Find("ResumeButton").gameObject;
            volumeButton = mainSceneCanvas.Find("VolumeButton").gameObject;
            exitButton = mainSceneCanvas.Find("ExitButton").gameObject;
            surePanel = mainSceneCanvas.Find("SurePanel").gameObject;
        }

        currentAudioImage = volumeButton.GetComponent<Image>();

        // Set audio volume.
        if (AudioListener.volume == 1) {
            currentAudioImage.sprite = audioOnSprite;
        } else {
            currentAudioImage.sprite = audioOffSprite;
        }

        // If scene is menu, load main scene asyncronously.
        if (SceneManager.GetActiveScene().name == "Menu") {
            SceneManager.LoadSceneAsync("Main");
        }
    }

    private void Update() {
        // When player presses Android's back button.
        if (Input.GetKey(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().name == "Main") {
                PauseButtonAction();
            }
            if (SceneManager.GetActiveScene().name == "Menu") {
                Application.Quit();
            }
        }
    }

    /// ///////////////////////////////////////////////////// ///
    /// Below are actions which are called from Menu scene.   ///
    /// ///////////////////////////////////////////////////// ///

    public void StartButtonAction() {
        SceneManager.LoadScene("Main");
    }

    /// ///////////////////////////////////////////////////// ///
    /// Below are actions which are called from Main scene.   ///
    /// ///////////////////////////////////////////////////// ///

    public void PauseButtonAction() {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        volumeButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void ResumeButtonAction() {
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        volumeButton.SetActive(false);
        exitButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void VolumeButtonAction() {
        if (AudioListener.volume == 1) {
            currentAudioImage.sprite = audioOffSprite;
            AudioListener.volume = 0;
        } else {
            currentAudioImage.sprite = audioOnSprite;
            AudioListener.volume = 1;
        }
    }

    // Exit button doesn't perform exit, it just opens sure panel.
    public void ExitButtonAction() {
        surePanel.SetActive(true);
        resumeButton.SetActive(false);
        volumeButton.SetActive(false);
        exitButton.SetActive(false);
    }

    // Sure panel decides if player stays in the scene or not.
    public void SurePanelAction(string buttonName) {
        if (buttonName == "Yes") {
            SceneManager.LoadScene("Menu");
            ResumeButtonAction();
        } else {
            surePanel.SetActive(false);
            resumeButton.SetActive(true);
            volumeButton.SetActive(true);
            exitButton.SetActive(true);
        } 
    }

    public void ReplayButtonAction() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
