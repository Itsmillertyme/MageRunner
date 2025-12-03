using System.Collections;
using UnityEngine;

public class DemoOutroPanelController : MonoBehaviour {
    #region Variables
    [Header("Fade Settings")]
    [SerializeField] private float defaultFadeDuration = 1.0f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;
    #endregion

    #region Unity Methods
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Start fully invisible
        canvasGroup.alpha = 0f;

        // Turn whole panel OFF until we're told to show it
        gameObject.SetActive(false);
    }
    #endregion

    #region Utility Methods
    public void ShowPanel() {
        ShowPanel(defaultFadeDuration);
    }

    public void ShowPanel(float duration) {
        // Ensure panel is active before fading
        gameObject.SetActive(true);

        // Kill previous fade if one exists
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeInRoutine(duration));
    }

    public void HidePanel() {
        HidePanel(defaultFadeDuration);
    }

    public void HidePanel(float duration) {
        // Stop any previous fade
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutRoutine(duration));
    }

    public void ReloadLevel() {
        // Reload the current active scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        HidePanel(0.33f);
    }

    public void ExitGame() {
        // Exit the application
        Application.Quit();
    }
    #endregion

    #region Coroutines
    private IEnumerator FadeInRoutine(float duration) {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        fadeRoutine = null;
    }

    private IEnumerator FadeOutRoutine(float duration) {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false); // fully hidden now
        fadeRoutine = null;
    }
    #endregion
}
