using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour {

    [Header("Audio")]
    [SerializeField] private AudioSource menuAudio;
    [SerializeField] private AudioClip startGameClip;
    [SerializeField] private MusicManager musicManager;

    [Header("Scene Settings")]
    [SerializeField] private string levelSceneName = "Level1_Dungeon"; // main level scene

    [Header("Misc Settings")]
    [SerializeField] Button continueButton;


    #region Unity Methods
    private void Awake() {
        // Always start on menu music playlist
        if (musicManager != null)
            musicManager.SwitchPlaylist(0);
    }

    private void Start() {
        if (continueButton != null) {
            SetContinueButtonState(SaveSystem.SaveExists());
        }
    }

    void SetContinueButtonState(bool isInteractable) {
        if (isInteractable) {
            continueButton.interactable = true;
            continueButton.transform.GetChild(1).gameObject.SetActive(true);
            continueButton.transform.GetChild(2).gameObject.SetActive(false);
        }
        else {
            continueButton.interactable = false;
            continueButton.transform.GetChild(1).gameObject.SetActive(false);
            continueButton.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    #endregion

    #region Utility Methods
    public void StartNewGame() {
        // Delete any existing save
        SaveSystem.DeleteSave();

        // Play audio and begin coroutine
        if (menuAudio != null && startGameClip != null) {
            menuAudio.clip = startGameClip;
            menuAudio.Play();
        }

        StartCoroutine(BeginGame(false));
    }
    public void ContinueGame() {
        if (!SaveSystem.SaveExists()) {
            Debug.LogWarning("[SplashManager] No save file found. Continue disabled.");
            return;
        }

        if (menuAudio != null && startGameClip != null) {
            menuAudio.clip = startGameClip;
            menuAudio.Play();
        }

        StartCoroutine(BeginGame(true));
    }
    private IEnumerator BeginGame(bool loadSave) {
        yield return new WaitForSeconds(startGameClip.length + 0.1f);

        // Pass flag to GameManager via PlayerPrefs
        if (loadSave)
            PlayerPrefs.SetInt("LoadSaveOnStart", 1);
        else
            PlayerPrefs.DeleteKey("LoadSaveOnStart");

        // Load the level scene
        SceneManager.LoadScene(levelSceneName);

        // Switch to gameplay music
        if (musicManager != null)
            musicManager.SwitchPlaylist(1);
    }

    #endregion

}
