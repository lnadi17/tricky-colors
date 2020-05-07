using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public Transform mainSceneCanvas;
    public GameObject gameOverPanel;
    public float surePanelFadeInSpeed;
    public float surePanelFadeOutSpeed;
    public float gameOverFadeInSpeed;
    public Sprite audioOnSprite;
    public Sprite audioOffSprite;
    public Text bestText;
    public Animator fadeAnimator;
    public Image fadeImage;
    public SpriteRenderer playerSpriteRenderer;

    private GameObject pauseButton;
    private GameObject resumeButton;
    private GameObject volumeButton;
    private GameObject exitButton;
    private GameObject surePanel;
    private CanvasGroup surePanelGroup;
    private CanvasGroup gameOverGroup;
    private Image currentAudioImage;
    private Text sureText;
    private Button yesButton;
    private Button noButton;

    private AsyncOperation asyncOperation;

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
            sureText = surePanel.transform.Find("SureText").GetComponent<Text>();
            yesButton = surePanel.transform.Find("YesButton").GetComponent<Button>();
            noButton = surePanel.transform.Find("NoButton").GetComponent<Button>();
            surePanelGroup = surePanel.GetComponent<CanvasGroup>();
            gameOverGroup = gameOverPanel.GetComponent<CanvasGroup>();
            currentAudioImage = volumeButton.GetComponent<Image>();

            // Set audio volume.
            if (AudioListener.volume == 1) {
                currentAudioImage.sprite = audioOnSprite;
            } else {
                currentAudioImage.sprite = audioOffSprite;
            }
        }

        // If scene is menu, load main scene asyncronously.
        asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false;
    }

    private void Update() {
        // When player presses Android's back button.
        if (Input.GetKey(KeyCode.Escape)) {
            // If in main scene, game pauses only if Time.timeScale is 1 and game is not over.
            if (SceneManager.GetActiveScene().name == "Main" && Time.timeScale == 1 && gameOverPanel.activeSelf != true) {
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
        fadeAnimator.SetBool("fade", true);
        StartCoroutine(SceneFadingOut("Main"));
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
        UpdateSurePanelColors();
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
        FadeIn(surePanelGroup, surePanel, surePanelFadeInSpeed);

        resumeButton.SetActive(false);
        volumeButton.SetActive(false);
        exitButton.SetActive(false);
    }

    // This happens if player decides to stay in the scene.
    public void ContinuePlayingAction() {
        FadeOut(surePanelGroup, surePanel, surePanelFadeOutSpeed);

        // They shouldn't be activated right away (causes a bug).
        resumeButton.SetActive(true);
        volumeButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void ReplayButtonAction() {
        fadeAnimator.SetBool("fade", true);
        StartCoroutine(SceneFadingOut("Main"));
    }

    public void GameOverAction() {
        pauseButton.SetActive(false);
        FadeIn(gameOverGroup, gameOverPanel, gameOverFadeInSpeed);
    }

    public void ExitToMenuAction() {
        // If game is paused, we need to resume it before switching to the next scene.
        Time.timeScale = 1;

        fadeAnimator.SetBool("fade", true);
        StartCoroutine(SceneFadingOut("Menu"));
    }

    /// ///////////////////////////////////////////// ///
    /// Below are fade in/out effect implementations. ///
    /// ///////////////////////////////////////////// ///

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, GameObject obj, float start, float end, float lerpValue = 0.5f) {
        cg.alpha = start;
        // Ensure that canvas group object is active.
        obj.SetActive(true);
        // While fading, buttons in this object's childs are not interactable.
        foreach (Button b in obj.GetComponentsInChildren<Button>()) {
            b.interactable = false;
        }

        while (true) {
            float currentValue = Mathf.Lerp(cg.alpha, end, lerpValue);
            cg.alpha = currentValue;

            if (Mathf.Abs(cg.alpha - end) < 0.01) {
                cg.alpha = end;
                break;
            }

            // Speed of fading is dependent on this value too.
            yield return new WaitForSecondsRealtime(0.025f);
        }

        // If canvas is completely transparent, disable it.
        if (end == 0) {
            obj.SetActive(false);
        }

        // Reenable button interaction.
        foreach (Button b in obj.GetComponentsInChildren<Button>()) {
            b.interactable = true;
        }
    }

    private void FadeIn(CanvasGroup cg, GameObject obj, float lerpValue) {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(cg, obj, 0, 1, lerpValue));
    }

    private void FadeOut(CanvasGroup cg, GameObject obj, float lerpValue) {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(cg, obj, 1, 0, lerpValue));
    }

    // Passed argument is the name of the scene which is going to load next.
    private IEnumerator SceneFadingOut(string sceneName) {
        yield return new WaitUntil(() => fadeImage.color.a == 1);

        // Main scene is loaded asynchronously, so it needs another type of call.
        if (sceneName == "Main") {
            asyncOperation.allowSceneActivation = true;
        } else {
            SceneManager.LoadScene(sceneName);
        }
    }

    void UpdateSurePanelColors() {
        Color currentPlayerColor = playerSpriteRenderer.color;
        // Pressed color in this project will always be slight tint to white.
        Color pressedColor = currentPlayerColor + new Color(0.1f, 0.1f, 0.1f);

        ColorBlock colorBlock = new ColorBlock {
            normalColor = currentPlayerColor,
            highlightedColor = currentPlayerColor,
            pressedColor = pressedColor,
            selectedColor = currentPlayerColor,
            disabledColor = currentPlayerColor,
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        sureText.color = currentPlayerColor;
        yesButton.colors = noButton.colors = colorBlock;
    }
}
